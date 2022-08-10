using IoTGateway.Controllers;
using IoTGateway.Data;
using IoTGateway.Models;
using IoTGateway.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IoTGateway.Services.Implementations
{
    public class GatewayService : IGatewayService
    {
        private ApplicationDbContext Context { get; }

        public GatewayService(ApplicationDbContext context, IPeripheralService peripheralService)
        {
            Context = context;
            if (!Context.Gateways.Any())
            {
                var names = new[] { "ALPHA", "CHARLIE", "DELTA", "FOX", "TANGO","TAU","OMEGA","EPSILON" };
                var vendors = new[] { "FUJISTU", "HP", "LENOVO", "APPLE", "LOGITECH", "SEAGATE", "HYDREKA" };
                var rn = new Random();
                foreach (var i in names)
                {
                    var gw = new Gateway()
                    {
                        IPAddress = $"{rn.Next(10, 254)}.{rn.Next(10, 254)}.{rn.Next(10, 254)}.{rn.Next(10, 254)}",
                        Name = i,
                        UID = Guid.NewGuid().ToString(),
                        Visible = true,
                        Peripherals = new List<Peripheral>()
                    };
                    Context.Gateways.Add(gw);
                    Context.SaveChanges();
                    for (int j = 0; j < rn.Next(6,10); j++)
                    {
                        var m = peripheralService.AddPeripheral(new PeripheralModel() {gatewayId = gw.Id, uid = rn.Next(10000, 99999), status = rn.Next(0,2), nvendor = vendors[rn.Next(vendors.Length)] }).Result;
                        Console.WriteLine(m.Result);
                    }
                }
            }
        }


        public async Task<TaskResult> AddGateway(GatewayModel gateway)
        {
            Gateway gat = null;
            if (string.IsNullOrEmpty(gateway.IPAddress)|| !System.Net.IPAddress.TryParse(gateway.IPAddress,out System.Net.IPAddress m))
            {
                var r = TaskResult.Failure;
                r.ErrorMessages.Add("Invalid or empty IPAddress");
                return r;
            }
            if (await Context.Gateways.AnyAsync(i=>i.UID == gateway.UID))
            {
                var r = TaskResult.Failure;
                r.ErrorMessages.Add("Unique ID duplicated: "+gateway.UID);
                return r;
            }
            gat = Gateway.FromModel(gateway);
            gat.IPAddress = m.ToString();
            await Context.Gateways.AddAsync(gat);
            await Context.SaveChangesAsync();
            var rx = TaskResult.Success;
            rx.Item = gat;
            return rx;
        }

        public async Task<Gateway[]> GetGateways()
        {
            var vendors = await Context.Vendors.AsNoTracking().ToDictionaryAsync(i => i.Id, i => i);
            var list = await Context.Gateways.Include(i => i.Peripherals).AsNoTrackingWithIdentityResolution().AsSplitQuery().ToArrayAsync();
            foreach (var item in list)
            {
                foreach (var per in item.Peripherals)
                {
                    per.Vendor = vendors.ContainsKey(per.VendorId)?vendors[per.VendorId]:null;
                    per.Gateway = null;
                }
                item.Peripherals = item.Peripherals.ToArray();
            }
            return list;
        }


        public async Task<Gateway> GatewayDeitals(int id)
        {
            if (await Context.Gateways.AnyAsync(i => i.Id == id))
            {
                return await Context.Gateways.Include(i => i.Peripherals).ThenInclude(i => i.Vendor).FirstAsync(i => i.Id == id);
            }
            return null;
        }

        public async Task<Vendor[]> GetVendors()
        {
            return await Context.Vendors.OrderBy(i => i.Name).AsNoTracking().ToArrayAsync();
        }
    }
}
