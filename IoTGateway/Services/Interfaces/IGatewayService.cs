using IoTGateway.Controllers;
using IoTGateway.Models;

namespace IoTGateway.Services.Interfaces
{
    public interface IGatewayService
    {
        /// <summary>
        /// All gateways and peripherals
        /// </summary>
        /// <returns></returns>
        Task<Gateway[]> GetGateways();

        /// <summary>
        /// Adds a gateway
        /// </summary>
        /// <param name="gateway">Gateway data</param>
        /// <returns></returns>
        Task<TaskResult> AddGateway(GatewayModel gateway);

        /// <summary>
        /// Displays all info about a gateway and peripherals within
        /// </summary>
        /// <param name="id">Gateway identity</param>
        /// <returns></returns>
        Task<Gateway> GatewayDeitals(int id);
        Task<Vendor[]> GetVendors();
    }
}
