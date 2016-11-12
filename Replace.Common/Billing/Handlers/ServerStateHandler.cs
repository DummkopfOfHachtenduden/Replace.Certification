using Replace.Common.Billing.Http;
using System.Net;

namespace Replace.Common.Billing.Handlers
{
    internal class ServerStateHandler : FilteredHttpHandler
    {
        public ServerStateHandler(BillingManager manager) : base(manager)
        {
        }

        public override bool Handle(HttpListenerContext context)
        {
            base.Handle(context);

            if (context.Request.Url.LocalPath.ToLower() != "/billing_serverstate.asp")
                return false;

            base.SendResult(context.Response, "1");

            return true;
        }
    }
}