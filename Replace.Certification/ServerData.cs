using Replace.Common.Security;

namespace Replace.Certification
{
    internal class ServerData
    {
        public SecurityManager Security { get; set; }
        public bool Connected { get; set; }
        public CertificationManager CertificationManager { get; set; }

        public ServerData()
        {
            this.Security = new SecurityManager();
            this.Security.GenerateSecurity(false, false, false);
        }
    }
}