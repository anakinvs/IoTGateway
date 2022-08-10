using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTGateway.Models
{
    public abstract class BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool Visible { get; set; } = false;

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
