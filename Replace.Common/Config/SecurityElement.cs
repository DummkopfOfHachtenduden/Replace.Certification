using System.Xml;

namespace Replace.Common.Config
{
    public class SecurityElement
    {
        public bool Blowfish { get; set; }
        public bool CRC { get; set; }
        public bool Handshake { get; set; }

        public SecurityElement(XmlNode node)
        {
            this.Blowfish = bool.Parse(node.Attributes[nameof(this.Blowfish)].Value);
            this.CRC = bool.Parse(node.Attributes[nameof(this.CRC)].Value);
            this.Handshake = bool.Parse(node.Attributes[nameof(this.Handshake)].Value);
        }
    }
}