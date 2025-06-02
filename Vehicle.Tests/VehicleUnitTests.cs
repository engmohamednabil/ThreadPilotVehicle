using Microsoft.Extensions.Logging;
using Moq;
using Vehicle.Controllers;
using Vehicle.Core.Interfaces;
using Vehicle.Core.Services;

namespace Vehicle.Tests
{
    public class VehicleUnitTests
    {
        private ILogger<VehicleController> _logger;
        private IVehicleService _vehicleService;
        private VehicleController _vehicleController;

        [SetUp]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<VehicleController>>();
            _vehicleService = new VehicleService();
            _vehicleController =
                new VehicleController(_logger, _vehicleService);
        }

        [TestCase(36774)]
        public void TestGetVehicleByRegNumIfVehicleExist(int regnum)
        {
            var response = _vehicleService.GetVehicleByRegNum(regnum);
            Assert.That(response.Result.VehicleModel, Is.EqualTo("Chevrolet"));
        }

        [TestCase(10000)]
        public void TestGetVehicleByRegNumReturnEmptyFromServiceIfNotExist(int regnum)
        {
            var response = _vehicleService.GetVehicleByRegNum(regnum);
            Assert.That(response.Result.VehicleModel, Is.EqualTo(""));
        }

        [TestCase(10000)]
        public void TestGetVehicleByRegNumReturnErrorFromControllerIfNotExist(int regnum)
        {
            var exception = Assert.ThrowsAsync<NotFoundException>(() => _vehicleController.GetVehicleByRegNum(regnum));
            Assert.AreEqual("Vehicle not found.", exception.Message);
        }
    }
}