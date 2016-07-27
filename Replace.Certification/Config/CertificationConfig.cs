using Replace.Common.Config;
using System.Xml;

namespace Replace.Certification.Config
{
    public class CertificationConfig : ConfigFile
    {
        public DatabaseElement Database { get; set; }
        //public SecurityElement Security { get; private set; }
        //public BindingElement Binding { get; private set; }

        public CertificationConfig(string fileName, string root) : base(fileName, root)
        {
            foreach (XmlNode node in this.RootElement)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                switch (node.Name)
                {
                    case nameof(this.Database):
                        this.Database = new DatabaseElement(node);
                        break;

                        //case nameof(this.Security):
                        //    this.Security = new SecurityElement(node);
                        //    break;

                        //case nameof(this.Binding)
                        //        this.Binding = new BindingElement(node);
                        //    break;
                }
            }
        }
    }
}