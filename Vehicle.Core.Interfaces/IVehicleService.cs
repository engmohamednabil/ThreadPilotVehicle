using Vehicle.Core.Models;

namespace Vehicle.Core.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleResponse> GetVehicleByRegNum(int RegNum);
    }
}
