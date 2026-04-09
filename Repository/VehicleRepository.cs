using Microsoft.EntityFrameworkCore;
using RentaCarAPI.Controllers;
using RentaCarAPI.Data;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;

namespace RentaCarAPI.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly RDataContext _context;
        public VehicleRepository(RDataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Vehicle>> GetVehicles()
        {
            return await _context.vehicles
                .Include(v => v.Depot)
                .ThenInclude(d => d.Location)
                .OrderBy(v => v.VehId)
                .ToListAsync();
        }
        public async Task<Vehicle> GetVehicle(int VehId)
        {
            return await _context.vehicles
                .Include(d => d.Depot)
                .Where(v => v.VehId == VehId).FirstOrDefaultAsync();
        }

        public async Task<Vehicle> GetVehicle(string Model)
        {
            return await _context.vehicles.Where(v => v.Model == Model).FirstOrDefaultAsync();
        }

        public async Task<int> GetVehicleRentedDays(int VehId)
        {
            return await _context.rentals.Where(v => v.VehId == VehId).SumAsync( r => r.DurationDays);
        }
        public async Task<bool> VehicleExists(int VehId)
        {
            return await _context.vehicles.AnyAsync(v => v.VehId == VehId);
        }

        public async Task<bool> CreateVehicle(Vehicle vehicle)
        {
            await _context.vehicles.AddAsync(vehicle);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateVehicle(Vehicle vehicle)
        {
            _context.Entry(vehicle).State = EntityState.Detached;
            _context.vehicles.Update(vehicle);
            return  Save();
        }
    }
}
