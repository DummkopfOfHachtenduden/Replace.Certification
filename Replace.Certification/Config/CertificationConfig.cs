using Replace.Common.Billing.Model;

namespace Replace.Certification.Config
{
    public class CertificationConfig
    {
        /// <summary>
        /// "Data Source={Server};Initial Catalog={Name};User ID={User};Password={Password}"
        /// </summary>
        public string CertificationConnectionString { get; set; }

        public BillingConfig Billing { get; set; }
    }
}