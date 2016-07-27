using NLog;
using Replace.Common;
using Replace.Common.AsyncNetwork;
using Replace.Common.Certification;
using System;
using System.IO;
using System.Threading;

namespace Replace.Certification
{
    internal class Program
    {
        public const string ModuleName = "Certification";

        private static Logger Logger = LogManager.GetLogger(ModuleName);

        private static void Main(string[] args)
        {
            Helper.SetupConsole(120, Console.BufferHeight);

            var configFileName = Path.Combine(Environment.CurrentDirectory, "Config", ModuleName + ".xml");
            var config = new Config.CertificationConfig(configFileName, ModuleName);

            var certificationManager = new CertificationManager(config);
            certificationManager.Load(ModuleName);

            var network = new AsyncServer();
            network.Accept(certificationManager.CertificationMachine.PrivateIP,
                            certificationManager.CertificationBody.BindPort,
                            5,
                            new ServerInterface(),
                            certificationManager);

            Logger.Info($"Certification server started on {certificationManager.CertificationMachine.PrivateIP}:{certificationManager.CertificationBody.BindPort}");
            Logger.Info("Press ESC to exit...");

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;
                }

                network.Tick();
                Thread.Sleep(1);
            }
            certificationManager.CertificationBody.State = ServerBodyState.Exit;
        }
    }
}