using NLog;
using Replace.Common.Billing.Handlers;
using Replace.Common.Billing.Http;
using Replace.Common.Billing.Model;
using Replace.Common.Database;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Replace.Common.Billing
{
    public class BillingManager
    {
        private static Logger BillingLogger = LogManager.GetLogger(nameof(BillingManager));

        private SqlDatabase _database;
        private HttpServer _server;

        public BillingConfig Config { get; private set; }

        public SilkData GetSilkData(int UserJID)
        {
            SqlDataReader reader;

            var queryBuilder = new StringBuilder();
            queryBuilder.Append("DECLARE @ReturnValue int ");
            queryBuilder.Append("DECLARE @SilkOwn int ");
            queryBuilder.Append("DECLARE @SilkGift int ");
            queryBuilder.Append("DECLARE @Mileage int ");
            queryBuilder.AppendFormat("EXEC @ReturnValue = _GetSilkDataForGameServer {0}, @SilkOwn OUTPUT, @SilkGift OUTPUT, @Mileage OUTPUT ", UserJID);
            queryBuilder.Append("SELECT @ReturnValue, @SilkOwn, @SilkGift, @Mileage");

            if (_database.Execute(queryBuilder.ToString(), out reader) == false)
                throw new Exception("Error querying database");

            using (reader)
            {
                if (reader.Read() == false)
                    throw new Exception("Error reading value");

                if (reader.GetInt32(0) != 0)
                    throw new Exception("Error getting value");

                if (BillingLogger.IsTraceEnabled)
                    BillingLogger.Trace("_GetSilkDataForGameServer ({0}): {1} {2} {3} {4}", UserJID, reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));

                return new SilkData(reader);
            }
        }

        public BillingManager(BillingConfig config)
        {
            this.Config = config;

            _database = new SqlDatabase();
            _server = new HttpServer();
        }

        public void Load()
        {
            _server.Prefixes.Add($"http://{this.Config.IP}:{this.Config.Port}/");
            _server.Handlers.Add(new ServerStateHandler(this));
            _server.Handlers.Add(new SilkDataCallHandler(this));

            _database.Open(this.Config.ConnectionString);

            _server.Start();
        }
    }
}