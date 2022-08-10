using IoTGateway.Controllers;
using IoTGateway.Models;

namespace IoTGateway.Services.Interfaces
{
    public interface IPeripheralService
    {
        /// <summary>
        /// Adds a peripheral to a gateway
        /// </summary>
        /// <param name="mperipheral"></param>
        /// <returns></returns>
        Task<TaskResult> AddPeripheral(PeripheralModel mperipheral);

        /// <summary>
        /// Removes a peripheral from a gateway
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult> RemovePeripheral(int id);
    }
}
