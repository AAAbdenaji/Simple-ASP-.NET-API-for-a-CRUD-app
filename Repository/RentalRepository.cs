using Microsoft.EntityFrameworkCore;
using RentaCarAPI.Data;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;

namespace RentaCarAPI.Repository
{
    public class RentalRepository :IRentalRepository
    {
        private readonly RDataContext _context;
        public RentalRepository(RDataContext context)
        {
            _context = context;
        }
        public rental GetRental(Guid ClientId,int VehId,DateTime DoR)
        {
            return _context.rentals.Where(r => r.ClientId == ClientId && r.VehId == VehId && r.DateOfRental == DoR).FirstOrDefault();
        }
        public ICollection<rental> GetRentalsOfClient(Guid Clientid)
        {
            return _context.rentals.Where(r => r.ClientId == Clientid).ToList();
        }
        public ICollection<rental> GetRentalsOfVehicles(int VehId)
        {
            return _context.rentals.Where(r => r.VehId == VehId).ToList();
        }
        public ICollection<rental> GetRentals()
        {
            return _context.rentals
                .Include(r => r.Client)
                .ThenInclude(c => c.Location)
                .Include(r => r.Vehicle)
                .ThenInclude(v => v.Depot)
                .ThenInclude(d => d.Location)
                .OrderBy(c => c.ClientId)
                .ToList();
        }

        public bool rentalExists(Guid Clientid, int vehid, DateTime DoR)
        {
            return _context.rentals.Any(r => r.ClientId == Clientid && r.VehId == vehid && r.DateOfRental == DoR);
        }
    }
}
