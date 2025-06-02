using Vehicle.Core.Interfaces;
using Vehicle.Core.Models;
using VehicleObj = Vehicle.Core.Entities.Vehicle;

namespace Vehicle.Core.Services
{
    public class VehicleService : IVehicleService
    {
        private static readonly VehicleObj[] vehicles =
        [
            new VehicleObj { VehicleRegNum = 23432, VehicleModel = "Toyota", VehicleProdYear = "2020" },
            new VehicleObj { VehicleRegNum = 45634, VehicleModel = "Honda", VehicleProdYear = "2019" },
            new VehicleObj { VehicleRegNum = 36436, VehicleModel = "Ford", VehicleProdYear = "2021" },
            new VehicleObj { VehicleRegNum = 36774, VehicleModel = "Chevrolet", VehicleProdYear = "2018" },
            new VehicleObj { VehicleRegNum = 45774, VehicleModel = "Dodge", VehicleProdYear = "2022" },
            new VehicleObj { VehicleRegNum = 74757, VehicleModel = "Nissan", VehicleProdYear = "2020" },
            new VehicleObj { VehicleRegNum = 75747, VehicleModel = "Jeep", VehicleProdYear = "2023" },
            new VehicleObj { VehicleRegNum = 97964, VehicleModel = "Subaru", VehicleProdYear = "2017" },
            new VehicleObj { VehicleRegNum = 66832, VehicleModel = "Mazda", VehicleProdYear = "2021" }
        ];

        public Task<VehicleResponse> GetVehicleByRegNum(int regNum)
        {
            var vehicle = vehicles.FirstOrDefault(v => v.VehicleRegNum == regNum);
            if (vehicle == null)
            {
                return Task.FromResult(new VehicleResponse
                {
                    VehicleModel = "",
                    VehicleProdYear = "",
                });
            }

            return Task.FromResult(new VehicleResponse
            {
                VehicleModel = vehicle.VehicleModel,
                VehicleProdYear = vehicle.VehicleProdYear,
            });
        }
    }
}
