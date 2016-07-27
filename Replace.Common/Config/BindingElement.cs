using System.Xml;

namespace Replace.Common.Config
{
    public class BindingElement
    {
        public string IP { get; set; }
        public ushort Port { get; set; }

        public BindingElement(XmlNode node)
        {
            this.IP = node.Attributes[nameof(this.IP)].Value;
            this.Port = ushort.Parse(node.Attributes[nameof(this.Port)].Value);
        }
    }
}