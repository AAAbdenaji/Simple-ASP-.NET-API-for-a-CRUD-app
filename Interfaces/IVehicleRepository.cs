using RentaCarAPI.Models;

namespace RentaCarAPI.Interfaces
{
    public interface IVehicleRepository
    {
        Task<ICollection<Vehicle>> GetVehicles();
        Task<Vehicle> GetVehicle(int VehId);
        Task<Vehicle> GetVehicle(string Model);
        Task<int> GetVehicleRentedDays(int VehId);
        Task<bool> VehicleExists(int VehId);
        Task<bool> CreateVehicle(Vehicle vehicle);
        bool UpdateVehicle(Vehicle vehicle);
        bool Save();
    }
}
