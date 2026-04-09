using Microsoft.EntityFrameworkCore;
using RentaCarAPI.Data;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;

namespace RentaCarAPI.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly RDataContext _context;
        public LocationRepository(RDataContext context)
        {
            _context = context;
        }

        public async Task<Location> GetLocation(int LocId)
        {
            return await _context.locations.Where(l => l.LocId == LocId).FirstOrDefaultAsync(); 
        }

        public async Task<Location> GetLocation(string LocName)
        {
            return await _context.locations.Where(l => l.LocName == LocName).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Location>> GetLocations()
        {
            return await _context.locations.OrderBy(l => l.LocId).ToListAsync();
        }

        public async Task<bool> LocationExists(int locId)
        {
            return await _context.locations.AnyAsync(l => l.LocId == locId);
        }
        public async Task<bool> CreateLocation(Location location)
        {
            await _context.locations.AddAsync(location);
            return await Save();
        }
        public async Task<bool>Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
        public async Task<bool> UpdateLocation(Location location)
        {
            _context.Entry(location).State = EntityState.Detached;
            _context.locations.Update(location);
            return await Save();
        }
    }
}
