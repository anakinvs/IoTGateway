namespace IoTGateway.Models
{
    public class PeripheralStatus: BaseModel
    {
        public int PeripheralId { get; set; }
        public Peripheral Peripheral { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public DateTime DateTime { get; set; }
    }


    /*
     modelBuilder
    .Entity<NetworkStuff>()
    .Property(e => e.IpAddress1)
    .HasConversion(
        v => v.ToString(),
        v => IPAddress.Parse(v));
     */
}
