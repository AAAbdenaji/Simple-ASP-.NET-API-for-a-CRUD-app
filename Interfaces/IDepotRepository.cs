using RentaCarAPI.Models;
using System.Collections;

namespace RentaCarAPI.Interfaces
{
    public interface IDepotRepository
    {
        Task<ICollection<Depot>> GetDepots();
        Task<Depot> GetDepot(int DepId);
        Task<Depot> GetDepot(string DepName);
        Task<bool> DepotExists(int DepId);
        Task<bool> CreateDepot(Depot depot);
        Task<bool> UpdateDepot(Depot depot);
    }
}
