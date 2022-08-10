using System.Formats.Asn1;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace IoTGateway.Models
{

    public class Gateway : BaseModel
    {
        public string UID { get; set; }

        public string IPAddress { get; set; } = "127.0.0.1";

        public string Name { get; set; }
        public ICollection<Peripheral> Peripherals { get; set; }

        internal static Gateway FromModel(GatewayModel gateway)
        {
            if (System.Net.IPAddress.TryParse(gateway.IPAddress, out System.Net.IPAddress address) && address != null && address.ToString().Contains("."))
            {
                var g = new Gateway
                {
                    UID = gateway.UID,
                    IPAddress = gateway.IPAddress,
                    Name = gateway.Name,
                    Visible = true
                };
                return g;
            }
            return null;
        }
    }

    public class GatewayModel
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
    }

    public class IPAddressConverter : JsonConverter<IPAddress>
    {
        public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.Read();
            return IPAddress.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
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
