using System.Data;

namespace Replace.Common.Billing.Model
{
    public class SilkData
    {
        public int SilkOwn { get; set; }
        public int SilkGift { get; set; }
        public int Mileage { get; set; }

        public SilkData(IDataRecord record)
        {
            this.SilkOwn = (int)record[1];
            this.SilkGift = (int)record[2];
            this.Mileage = (int)record[3];
        }
    }
}