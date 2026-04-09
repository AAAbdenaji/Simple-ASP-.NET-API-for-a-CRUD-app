using RentaCarAPI.Models;

namespace RentaCarAPI.Interfaces
{
    public interface ILocationRepository
    {
        Task<ICollection<Location>> GetLocations();
        Task<Location> GetLocation(int LocId);
        Task<Location> GetLocation(string LocName);
        Task<bool> LocationExists(int locId);
        Task<bool> CreateLocation(Location location);
        Task<bool> UpdateLocation(Location location);
    }
}
