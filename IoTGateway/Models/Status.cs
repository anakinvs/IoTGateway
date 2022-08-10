namespace IoTGateway.Models
{
    public enum Status
    {
        online,
        offline
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
