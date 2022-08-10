namespace IoTGateway.Models
{
    public class Vendor : BaseModel
    {
        public string Name { get; set; }

        public ICollection<Peripheral> Peripherals { get; set; }
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
