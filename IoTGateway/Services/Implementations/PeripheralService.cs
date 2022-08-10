using IoTGateway.Controllers;
using IoTGateway.Data;
using IoTGateway.Models;
using IoTGateway.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IoTGateway.Services.Implementations
{
    public class PeripheralService: IPeripheralService
    {
        private ApplicationDbContext Context { get; }
        public IConfiguration Configuration { get; }

        public PeripheralService(ApplicationDbContext context, IConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
            Context.SaveChangesFailed += Context_SaveChangesFailed;
        }

        private void Context_SaveChangesFailed(object? sender, SaveChangesFailedEventArgs e)
        {
            
        }

        public async Task<TaskResult> AddPeripheral(PeripheralModel mperipheral)
{
            var allowed = Configuration.GetValue<int>("MaxPeripheralsPerGateway");
            var gateway = mperipheral.gatewayId;
            if (await Context.Peripherals.CountAsync(i => i.GatewayId == gateway) + 1 > allowed)
            {
                var r = TaskResult.Failure;
                r.ErrorMessages.Add($"More than {allowed} peripherals per gateway are not allowed");
                return r;
            }
            if (await Context.Peripherals.AnyAsync(i => i.UID == mperipheral.uid))
            {
                var r = TaskResult.Failure;
                r.ErrorMessages.Add($"Unique ID duplicated {mperipheral.uid}");
                return r;
            }
            var peripheral = Peripheral.FromModel(mperipheral);
            var vendor = (await Context.Vendors.AnyAsync(i => i.Name == mperipheral.nvendor)) ?
                await Context.Vendors.FirstAsync(i => i.Name == mperipheral.nvendor) : new Vendor() { Name = mperipheral.nvendor };
            if (vendor.Id == 0)
            {
                await Context.AddAsync(vendor);
                await Context.SaveChangesAsync();
            }
            peripheral.Vendor = vendor;
            await Context.Peripherals.AddAsync(peripheral);
            await Context.SaveChangesAsync();
            var rx = TaskResult.Success;
            peripheral.Gateway = null;
            peripheral.Vendor.Peripherals = null;
            rx.Item = peripheral;
            return rx;
        }

        public async Task<TaskResult> RemovePeripheral(int id)
        {
            var per = await Context.Peripherals.FindAsync(id);
            if (per == null)
            {
                var res = TaskResult.Failure;
                res.ErrorMessages.Add($"Peripheral not found: {id}");
                return res;
            }
            Context.Peripherals.Remove(per);
            await Context.SaveChangesAsync();
            return TaskResult.Success;
        }
    }
}
