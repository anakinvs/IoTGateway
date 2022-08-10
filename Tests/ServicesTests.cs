using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests
{
    public class ServicesTests : UnitTestBase
    {
        IoTGateway.Services.Interfaces.IGatewayService serv;
        IoTGateway.Services.Interfaces.IPeripheralService per;
        IConfiguration configuration;

        public ServicesTests() : base()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"MaxPeripheralsPerGateway",  "10"},
            };
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            per = new IoTGateway.Services.Implementations.PeripheralService(context, configuration);
            serv = new IoTGateway.Services.Implementations.GatewayService(context, per);
        }

        [Fact]
        public async Task TestAddGateway()
        {
            var r = await serv.AddGateway(new GatewayModel() { IPAddress = "127.0.0.1", Name = "Test", UID = Guid.NewGuid().ToString() });
            Assert.True(r.Result);
        }

        [Fact]
        public async Task TestAddGatewaytoFailIP()
        {
            var r = await serv.AddGateway(new GatewayModel() { IPAddress = "a.b.c.1", Name = "Test", UID = Guid.NewGuid().ToString() });
            Assert.False(r.Result);
        }

        Random random = new Random();

        [Fact]
        public async Task TestAddPeripheral()
        {
            var gat = await context.Gateways.FirstAsync();
            var r = await per.AddPeripheral(new PeripheralModel() { uid = random.Next(1000000,9999999), nvendor = "TestX", status = 0 , gatewayId = gat.Id});
            Assert.True(r.Result);
        }

        [Fact]
        public async Task TestAddPeripheralOverflow()
        {
            var fgat = await context.Gateways.FirstAsync();
            for (int i = 0; i <= configuration.GetValue<int>("MaxPeripheralsPerGateway")+1; i++)
            {
                var uid = random.Next(1000000, 9999999);
                var r = await per.AddPeripheral(new PeripheralModel() { uid = uid, nvendor = "TestX"+uid, status = 0, gatewayId = fgat.Id });
                if (!r.Result)
                    Assert.False(r.Result);
            }
        }

        [Fact]
        public async Task TestAddPeripherNoDuplicateVendor()
        {
            var tx = "TestX";
            for (int i = 0; i <= 2; i++)
            {
                var uid = random.Next(1000000, 9999999);
                var r = await per.AddPeripheral(new PeripheralModel() { uid = uid, nvendor = tx, status = 0, gatewayId = context.Gateways.First().Id });
                if (!r.Result)
                    Assert.False(r.Result);
            }
            Assert.False(await context.Vendors.CountAsync(i => i.Name == tx) > 1);
        }

    }
}