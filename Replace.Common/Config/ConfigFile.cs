using System.Xml;

namespace Replace.Common.Config
{
    public abstract class ConfigFile
    {
        public XmlElement RootElement { get; private set; }

        public ConfigFile(string fileName, string root)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            this.RootElement = xmlDocument[root];
        }
    }
}