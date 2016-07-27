using NLog;
using Replace.Common;
using Replace.Common.AsyncNetwork;
using Replace.Common.Certification;
using Replace.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Replace.Certification
{
    internal class ServerInterface : IAsyncInterface
    {
        private static Logger Logger = LogManager.GetLogger(nameof(ServerInterface));

        public bool OnConnect(AsyncContext context)
        {
            ServerData context_data = new ServerData();

            context_data.CertificationManager = context.User as CertificationManager;
            context_data.Connected = true;

            context.User = context_data;

            return true;
        }

        public bool OnReceive(AsyncContext context, byte[] buffer, int count)
        {
            ServerData context_data = (ServerData)context.User;

            try
            {
                context_data.Security.Recv(buffer, 0, count);
                List<Packet> packets = context_data.Security.TransferIncoming();
                if (packets != null)
                {
                    foreach (Packet packet in packets)
                    {
                        switch (packet.Opcode)
                        {
                            case 0x5000:
                            case 0x9000:
                                continue;

                            case 0x2001:
                                OnModuleIdentification(packet, context_data);
                                break;

                            case 0x6003:
                                OnCertificationRequest(packet, context_data);
                                break;

                            case 0x2005:
                                OnServerUpdate(packet, context_data);
                                break;

                            case 0x6005:
                                OnServerUpdateRequest(packet, context_data);
                                break;

                            case 0x6008:
                                OnForwardRequest(packet, context_data);
                                break;

                            default:
                                byte[] payload = packet.GetBytes();
                                Console.WriteLine("[{7}][{0:X4}][{1} bytes]{2}{3}{4}{5}{6}", packet.Opcode, payload.Length, packet.Encrypted ? "[Encrypted]" : "", packet.Massive ? "[Massive]" : "", Environment.NewLine, payload.HexDump(), Environment.NewLine, context.Guid);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        private void OnServerUpdateRequest(Packet packet, ServerData context_data)
        {
            var updateFlag = (ServerUpdateType)packet.ReadByte();
            if (updateFlag.HasFlags(ServerUpdateType.Body))
            {
                var serverUpdate = new Packet(0x2005, false, true);
                serverUpdate.WriteByte(ServerUpdateType.Body);

                var unkByte0 = packet.ReadByte(); //checkByte = 0
                serverUpdate.WriteByte(unkByte0);
                while (true)
                {
                    var entryFlag = packet.ReadByte();
                    serverUpdate.WriteByte(entryFlag);
                    if (entryFlag == 2)
                        break;

                    var bodyID = packet.ReadUShort();
                    var body = context_data.CertificationManager.ServerBodyList.Single(p => p.ID == bodyID);

                    serverUpdate.WriteUShort(body.ID);
                    serverUpdate.WriteUInt(body.State);
                }

                context_data.Security.Send(serverUpdate);
            }
            if (updateFlag.HasFlags(ServerUpdateType.Cord))
            {
                var serverUpdate = new Packet(0x2005, false, true);
                serverUpdate.WriteByte(ServerUpdateType.Cord);

                var unkByte0 = packet.ReadByte(); //checkByte = 0
                serverUpdate.WriteByte(unkByte0);
                while (true)
                {
                    var entryFlag = packet.ReadByte();
                    serverUpdate.WriteByte(entryFlag);
                    if (entryFlag == 2)
                        break;

                    var cordID = packet.ReadUInt();
                    var cord = context_data.CertificationManager.ServerCordList.Single(p => p.ID == cordID);

                    serverUpdate.WriteUInt(cord.ID);
                    serverUpdate.WriteUInt(cord.State);
                }

                context_data.Security.Send(serverUpdate);
            }
        }

        private void OnModuleIdentification(Packet packet, ServerData context_data)
        {
            var moduleName = packet.ReadAscii();
            var moduleFlag = packet.ReadByte();

            if (moduleName == "GlobalManager") //&& moduleFlag == 1)
            {
                var ack = new Packet(packet.Opcode);
                ack.WriteAscii(context_data.CertificationManager.CertificationModule.Name);
                ack.WriteByte(moduleFlag);
                context_data.Security.Send(ack);
            }
            else
            {
                throw new Exception($"Unknown module identification: {moduleName} [{moduleFlag}]");
            }
        }

        private void OnCertificationRequest(Packet packet, ServerData context_data)
        {
            //TODO: Compare requested IP with sockets IP?
            var moduleName = packet.ReadAscii();
            var IP = packet.ReadAscii();

            var cert = context_data.CertificationManager;

            Module reqModule = cert.ModuleList.Single(p => p.Name == moduleName);
            ServerBody reqBody = null;

            foreach (var cord in cert.CertificationCords)
            {
                var body = cert.ServerBodyList.Single(p => p.ID == cord.ChildID);

                //Skip if moduleID does not match.
                if (body.ModuleID != reqModule.ID)
                    continue;

                //Skip if we are not certifier of that body (invalid or missing cord).
                if (body.CertifierID != cert.CertificationBody.ID)
                    continue;

                //Get assigned machine and skip if IPs do not match.
                var machine = cert.ServerMachineList.Single(p => p.ID == body.MachineID);
                if (machine.GetIP(cord.BindType) != IP)
                    continue;

                //We found the correct body!
                reqBody = body;
                break;
            }

            var ack = new Packet(0xA003, false, true);
            if (reqBody != null)
            {
                ack.WriteByte(1);
                cert.WriteAcknowledge(ack);

                //var buffer = ack.GetBytes();
                //File.WriteAllBytes(@"D:\Development\debug.dat", buffer);
            }
            else
            {
                ack.WriteByte(2);
                Logger.Error($"Can not certify {moduleName} [{IP}]");
            }
            context_data.Security.Send(ack);
        }

        private void OnServerUpdate(Packet packet, ServerData context_data)
        {
            var updateType = (ServerUpdateType)packet.ReadByte();

            if (updateType.HasFlags(ServerUpdateType.Body))
            {
                //ServerBody
                var unkByte0 = packet.ReadByte(); //checkByte = 0
                while (true)
                {
                    var entryFlag = packet.ReadByte();
                    if (entryFlag == 2)
                        break;

                    var serverBodyID = packet.ReadUShort();
                    var serverBodyState = (ServerBodyState)packet.ReadUInt();

                    var body = context_data.CertificationManager.ServerBodyList.Single(p => p.ID == serverBodyID);
                    var module = context_data.CertificationManager.ModuleList.Single(p => p.ID == body.ModuleID);
                    body.State = serverBodyState;
                    Logger.Info($"ServerBody#{body.ID} [{module.Name}] changed to {serverBodyState}");
                }
            }

            if (updateType.HasFlags(ServerUpdateType.Cord))
            {
                //ServerCord
                var unkByte0 = packet.ReadByte(); //check byte = 0
                while (true)
                {
                    var entryFlag = packet.ReadByte();
                    if (entryFlag == 2)
                        break;

                    var serverCordID = packet.ReadUInt();
                    var serverCordState = (ServerCordState)packet.ReadUInt();

                    var cord = context_data.CertificationManager.ServerCordList.Single(p => p.ID == serverCordID);
                    Logger.Info($"{cord} changed to {serverCordState}");
                }
            }
        }

        private void OnForwardRequest(Packet packet, ServerData context_data)
        {
            var forwardingID = packet.ReadUInt();
            var forwardingDestination = packet.ReadUShort(); //ServerBodyID
            var forwardedOpcode = packet.ReadUShort();

            var forwardAck = new Packet(0xA008, packet.Encrypted, packet.Massive);

            var result = forwardingDestination == context_data.CertificationManager.CertificationBody.ID;
            if (result)
            {
                forwardAck.WriteByte(1); //result
                forwardAck.WriteUInt(forwardingID);
                switch (forwardedOpcode)
                {
                    case 0x6310: //ShardUpdate
                        OnShardUpdate(packet, forwardAck, context_data);
                        break;

                    case 0x631C: //DivisionReport?
                        //OnDivisionReport(packet, forwardAck, context_data);
                        return;

                    default:
                        Logger.Warn($"Unknown forwardedOpcode [{forwardedOpcode.ToString("X4")}]: {packet}");
                        break;
                }
            }
            else
            {
                //return;
                //Lost the sample -.-
                forwardAck.WriteByte(2); //result
                forwardAck.WriteUInt(forwardingID);
                forwardAck.WriteUShort(0); //errorCode?
            }
            context_data.Security.Send(forwardAck);
        }

        //private void OnDivisionReport(Packet req, Packet ack, CertificationServerData context_data)
        //{
        //    ack.WriteUShort(0xA31C);

        //    var divisonID = req.ReadByte();
        //    //No idea how to respond to GlobalManager...

        //    Logger.Info($"Division {divisonID} reported.");
        //}

        private void OnShardUpdate(Packet req, Packet ack, ServerData context_data)
        {
            ack.WriteUShort(0xA310);

            short shardID = req.ReadShort();
            byte updateType = req.ReadByte();
            string shardName = string.Empty;
            short shardMaxUser = 0;
            bool result = false;

            if (updateType == 0)
            {
                //ShardName
                shardName = req.ReadAscii();
                result = context_data.CertificationManager.UpdateShardName(shardID, shardName);
            }
            else if (updateType == 1)
            {
                //MaxUser
                shardMaxUser = req.ReadShort();
                result = context_data.CertificationManager.UpdateShardMaxUser(shardID, shardMaxUser);
            }

            ack.WriteByte(result ? 1 : 2);
            ack.WriteShort(shardID);
            ack.WriteByte(updateType);
            if (updateType == 0)
                ack.WriteAscii(shardName);
            else if (updateType == 1)
                ack.WriteShort(shardMaxUser);
        }

        public void OnDisconnect(AsyncContext context)
        {
            ServerData context_data = (ServerData)context.User;
            context_data.Connected = false;
        }

        public void OnError(AsyncContext context, object user)
        {
            if (context != null && context.User != null)
            {
                ServerData context_data = (ServerData)context.User;
                context_data.Connected = false;
            }
        }

        public void OnTick(AsyncContext context)
        {
            ServerData context_data = (ServerData)context.User;
            if (context_data == null)
                return;

            if (!context_data.Connected)
                return;

            List<KeyValuePair<TransferBuffer, Packet>> buffers = context_data.Security.TransferOutgoing();
            if (buffers != null)
            {
                foreach (KeyValuePair<TransferBuffer, Packet> buffer in buffers)
                {
                    context.Send(buffer.Key.Buffer, 0, buffer.Key.Size);
                }
            }
        }
    }
}