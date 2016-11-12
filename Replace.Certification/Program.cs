using NLog;
using Replace.Certification.Config;
using Replace.Common;
using Replace.Common.AsyncNetwork;
using Replace.Common.Billing;
using Replace.Common.Certification;
using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace Replace.Certification
{
    internal class Program
    {
        public const string ModuleName = "Certification";

        private static Logger Logger = LogManager.GetLogger(ModuleName);

        private static CertificationManager certificationManager;
        private static BillingManager billingManager;

        private static void Main(string[] args)
        {
            Helper.SetupConsole(120, Console.BufferHeight);

            var configFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, "Config", ModuleName + ".xml"));

            using (var stream = configFile.OpenRead())
            {
                var serializer = new XmlSerializer(typeof(CertificationConfig));
                var config = serializer.Deserialize(stream) as CertificationConfig;
                if (config == null)
                {
                    Logger.Error("Failed to deserialize config");
                    Console.ReadLine();
                }

                certificationManager = new CertificationManager(config);
                billingManager = new BillingManager(config.Billing);
            }

            certificationManager.Load(ModuleName);
            billingManager.Load();

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