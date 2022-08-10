using IoTGateway.Data;
using IoTGateway.Models;
using IoTGateway.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTGateway.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GatewaysController : ControllerBase
    {

        private readonly ILogger<GatewaysController> _logger;
        private readonly IGatewayService _gatewayService;
        private readonly IPeripheralService _peripheralService;

        public GatewaysController(ILogger<GatewaysController> logger, IGatewayService gateway, IPeripheralService peripheral, IConfiguration configuration)
        {
            _logger = logger;
            _gatewayService = gateway;
            _peripheralService = peripheral;
            Configuration = configuration;
        }

        public ApplicationDbContext DbContext { get; }
        public IConfiguration Configuration { get; }

        [HttpGet(Name = "GetGateways")]
        public async Task<IEnumerable<Gateway>> GetGateways()
        {
            if (_logger!=null)
            _logger.LogInformation("Accessing list of gateways");
            return (await _gatewayService.GetGateways());
        }

        [HttpGet(Name = "MaxPeripheralsPerGateway")]
        public async Task<int> MaxPeripheralsPerGateway()
        {
            if (_logger != null)
                _logger.LogInformation("Configuration data");
            return Configuration.GetValue<int>("MaxPeripheralsPerGateway");
        }

        [HttpGet(Name = "GatewayDetails")]
        public async Task<Gateway> GatewayDetails(int id)
        {
            if (_logger != null)
                _logger.LogInformation($"Accessing to details of gateway {id}");
            return await _gatewayService.GatewayDeitals(id);
        }

        [HttpPost(Name = "AddGateway")]
        public async Task<TaskResult> AddGateway(GatewayModel gateway)
        {
            if (_logger != null)
                _logger.LogInformation("Registering gateway with params: {0},{1},{2}", gateway.Name, gateway.IPAddress, gateway.UID);
            return await _gatewayService.AddGateway(gateway);
        }

        [HttpPost(Name = "AddPeripheral")]
        public async Task<TaskResult> AddPeripheral(PeripheralModel mperipheral)
        {
            if (_logger != null)
                _logger.LogInformation("Registering peripheral " + mperipheral);
            return await _peripheralService.AddPeripheral(mperipheral);
        }

        [HttpPost(Name = "RemovePeripheral")]
        public async Task<TaskResult> RemovePeripheral(int mperipheral)
        {
            if (_logger != null)
                _logger.LogInformation("Removing peripheral " +mperipheral);
            return await _peripheralService.RemovePeripheral(mperipheral);
        }

        [HttpGet(Name = "GetVendors")]
        public async Task<Vendor[]> GetVendors()
        {
            if (_logger != null)
                _logger.LogInformation("Requesting vendors");
            return await _gatewayService.GetVendors();
        }
    }

    public class TaskResult
    {
        public bool Result { get; set; } = false;
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public object Item { get; set; }

        private TaskResult()
        {

        }

        public static TaskResult Success { get { return new TaskResult { Result = true }; } }
        public static TaskResult Failure { get { return new TaskResult(); } }
    }
}