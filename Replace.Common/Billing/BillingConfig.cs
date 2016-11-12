using System.Collections.Generic;
using System.Xml.Serialization;

namespace Replace.Common.Billing.Model
{
    public class BillingConfig
    {
        [XmlAttribute]
        public string IP { get; set; }

        [XmlAttribute]
        public ushort Port { get; set; }

        public string ConnectionString { get; set; }

        public List<string> WhitelistHosts { get; set; }

        public List<string> WhitelistIPs { get; set; }
    }
}