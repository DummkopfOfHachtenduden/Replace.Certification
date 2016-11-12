using System.Collections.Generic;
using System.Net;

namespace Replace.Common.Billing.Http
{
    internal class FilteredHttpHandler : IHttpHandler
    {
        private BillingManager _manager;
        protected BillingManager Manager => _manager;

        public FilteredHttpHandler(BillingManager manager)
        {
            _manager = manager;
        }

        public override bool Handle(HttpListenerContext context)
        {
            var clientIP = context.Request.RemoteEndPoint.Address;

            var resolvedIPs = new List<IPAddress>();
            foreach (var whiteHost in _manager.Config.WhitelistHosts)
                resolvedIPs.AddRange(Dns.GetHostAddresses(whiteHost));

            foreach (var whiteIP in _manager.Config.WhitelistIPs)
                resolvedIPs.Add(IPAddress.Parse(whiteIP));

            if (resolvedIPs.Contains(clientIP) == false)
                throw new AccessDeniedException(context, "Access to the resource was denied");

            return true;
        }
    }
}