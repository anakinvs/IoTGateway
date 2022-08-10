using System.ComponentModel.DataAnnotations.Schema;

namespace IoTGateway.Models
{
    public class Peripheral : BaseModel
    {
        public int UID { get; set; }

        [ForeignKey(nameof(Vendor))]
        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }

        public DateTime Created { get; set; }


        [ForeignKey(nameof(Gateway))]
        public int GatewayId { get; set; }

        public Gateway Gateway { get; set; }

        public Status Status { get; set; }

        internal static Peripheral FromModel(PeripheralModel mperipheral)
        {
            var resp = new Peripheral()
            {
                UID = mperipheral.uid,
                Created = DateTime.Now,
                GatewayId = mperipheral.gatewayId,
                Status = mperipheral.status == 0 ? Status.online : Status.offline
            };
            return resp;
        }

        
    }

    public class PeripheralModel
    {
        public int uid { get; set; }

        public int gatewayId { get; set; }

        public int status { get; set; }

        public string nvendor { get; set; }
    }
}
