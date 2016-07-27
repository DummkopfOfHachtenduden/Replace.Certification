using System.Xml;

namespace Replace.Common.Config
{
    public class DatabaseElement
    {
        public string Server { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string ConnectionString => $"Data Source={Server};Initial Catalog={Name};User ID={User};Password={Password}";

        public DatabaseElement(XmlNode node)
        {
            this.Server = node.Attributes[nameof(this.Server)].Value;
            this.Name = node.Attributes[nameof(this.Name)].Value;
            this.User = node.Attributes[nameof(this.User)].Value;
            this.Password = node.Attributes[nameof(this.Password)].Value;
        }
    }
}