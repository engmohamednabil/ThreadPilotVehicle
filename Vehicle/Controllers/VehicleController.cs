using Microsoft.AspNetCore.Mvc;
using Vehicle.Core.Interfaces;
using Vehicle.Core.Models;

namespace Vehicle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleService _vehicleService;

        public VehicleController(ILogger<VehicleController> logger,
            IVehicleService VehicleService)
        {
            _logger = logger;
            _vehicleService = VehicleService;
        }

        [HttpGet("/Vehicle/{regnum}")]
        public async Task<ActionResult<VehicleResponse>> GetVehicleByRegNum(int regnum)
        {
            _logger.LogInformation($"GET VehicleController.GetVehicleByRegNum");

            if (regnum <= 0 || regnum > int.MaxValue)
            {
                throw new ValidationException("Vehicle registration Number is incorreclty formatted.");
            }

            var response = await _vehicleService.GetVehicleByRegNum(regnum);
            if (response == null
                || string.IsNullOrEmpty(response.VehicleModel) 
                || string.IsNullOrEmpty(response.VehicleProdYear))
            {
                throw new NotFoundException("Vehicle not found.");
            }
            return Ok(response);
        }
    }
}
