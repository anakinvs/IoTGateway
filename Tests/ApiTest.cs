using IoTGateway.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ApiTest: UnitTestBase
    {
        IoTGateway.Services.Interfaces.IGatewayService? serv;
        IoTGateway.Services.Interfaces.IPeripheralService? per;
        IConfiguration? configuration;
        GatewaysController controller;

        public ApiTest(): base ()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"MaxPeripheralsPerGateway",  "10"},
            };
            configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
                .Build();
            per = new PeripheralService(context, configuration);
            serv = new GatewayService(context, per);
            controller = new GatewaysController(null, serv, per, configuration);
        }

        [Fact]
        public async Task GetGateways_ReturnsAnArrayOfGateways()
        {
            var acount = (await serv.GetGateways()).Count();
            var result = await controller.GetGateways();
            var viewResult = Assert.IsType<Gateway[]>(result);
            var model = Assert.IsAssignableFrom<Gateway[]>(viewResult);
            Assert.Equal(acount, model.Length);
        }

        [Fact]
        public async Task AddGateway_ReturnsTaskResultWithStoredGateway()
        {
            var modelx = new GatewayModel() { IPAddress = "1.2.3.4", Name = "testAdd", UID = "12354" };
            var result = await controller.AddGateway(modelx);
            var viewResult = Assert.IsType<TaskResult>(result);
            var model = Assert.IsAssignableFrom<TaskResult>(viewResult);
            Assert.True(model.Result);
            Assert.Equal((model.Item as Gateway).IPAddress, modelx.IPAddress);
            Assert.Equal((model.Item as Gateway).Name, modelx.Name);
            Assert.Equal((model.Item as Gateway).UID, modelx.UID);
            Assert.True((model.Item as Gateway).Id != 0);
        }

        [Fact]
        public async Task AddPepripheral_ReturnsTaskResultWithStoredPeripheral()
        {
            var first = (await serv.GetGateways()).First();
            var modelx = new PeripheralModel() { nvendor = "newvendor", uid = 234234, gatewayId = first.Id , status = 0};
            var result = await controller.AddPeripheral(modelx);
            var viewResult = Assert.IsType<TaskResult>(result);
            var model = Assert.IsAssignableFrom<TaskResult>(viewResult);
            Assert.True(model.Result);
            Assert.True((model.Item as Peripheral).Id != 0);
            Assert.Equal((model.Item as Peripheral).UID, modelx.uid);
            Assert.Equal((model.Item as Peripheral).Vendor.Name, modelx.nvendor);
            Assert.Equal((model.Item as Peripheral).Status, modelx.status==0?Status.online:Status.offline);
        }

    }
}
