using NLog;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Replace.Common.Billing.Handlers
{
    internal class SilkDataCallHandler : Http.FilteredHttpHandler
    {
        private static Logger BillingLogger = LogManager.GetLogger(nameof(BillingManager));

        private const string KEY = "SROG8Z_CDE1210598DK_AKD3HW1K04DL2-";

        private MD5 md5 = MD5.Create();

        public SilkDataCallHandler(BillingManager manager) : base(manager)
        {
        }

        private string Md5Sum(string plain)
        {
            var buffer = Encoding.ASCII.GetBytes(plain);
            var hash = md5.ComputeHash(buffer);

            var builder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                builder.Append(hash[i].ToString("x2"));

            return builder.ToString();
        }

        public override bool Handle(HttpListenerContext context)
        {
            base.Handle(context);

            if (context.Request.Url.LocalPath.ToLower() != "/billing_silkdatacall.asp")
                return false;

            int userJID;
            if (int.TryParse(context.Request.QueryString["JID"], out userJID))
            {
                var data = base.Manager.GetSilkData(userJID);

                string checksum = Md5Sum(string.Format("{0}.{1}.{2}.{3}.{4}", userJID, data.SilkOwn, data.SilkGift, data.Mileage, KEY));

                base.SendResult(context.Response, string.Format("1:{0},{1},{2},{3}", data.SilkOwn, data.SilkGift, data.Mileage, checksum));
            }
            else
            {
                BillingLogger.Warn($"{Caller.GetMemberName()}: Invalid JID format");
                base.SendResult(context.Response, "-2");
            }

            return true;
        }
    }
}