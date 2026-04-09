using Microsoft.EntityFrameworkCore;
using RentaCarAPI.Data;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;

namespace RentaCarAPI.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly RDataContext _context;
        public ClientRepository(RDataContext context)
        {
            _context = context;
        }

        public bool ClientExists(Guid Clientid)
        {
            return _context.clients.Any(c => c.Id == Clientid);
        }

        public Client GetClient(Guid Clientid)
        {
            return _context.clients.Where(c => c.Id == Clientid).FirstOrDefault();
        }

        public ICollection<Client> GetClients()
        {
            return _context.clients.OrderBy(c => c.Id).ToList();
        }
        public float GetClientTotalPaid(Guid Clientid)
        {
  
            var rentals = _context.rentals.Where(c => c.ClientId == Clientid);
            if (rentals.Count() == 0)
            {
                return 0;
            }
            return rentals.Sum(r => r.TotalPrice);
        }
    }
}
