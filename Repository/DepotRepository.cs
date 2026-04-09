using Microsoft.EntityFrameworkCore;
using RentaCarAPI.Data;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;

namespace RentaCarAPI.Repository
{
    public class DepotRepository : IDepotRepository
    {
        private readonly RDataContext _context;
        public DepotRepository(RDataContext context)
        {
            _context = context;
        }

        public async Task<bool> DepotExists(int DepId)
        {
            return await _context.depots.AnyAsync(d => d.DepId == DepId);
        }

        public async Task<Depot> GetDepot(int DepId)
        {
            return await _context.depots.Include(d => d.Location).Where(d => d.DepId == DepId).FirstOrDefaultAsync();
        }

        public async Task<Depot> GetDepot(string DepName)
        {
            return await _context.depots.Include(d => d.Location).Where(d => d.DepName == DepName).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Depot>> GetDepots()
        {
            return await _context.depots.Include(d => d.Location).OrderBy(d => d.DepId).ToListAsync();
        }
        public async Task<bool> CreateDepot(Depot depot)
        {
            await _context.depots.AddAsync(depot);
            return await Save();
        }
        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
        public async Task<bool> UpdateDepot(Depot depot)
        {
            _context.Entry(depot).State = EntityState.Detached;
            _context.depots.Update(depot);
            return await Save();
        }
    }
}
