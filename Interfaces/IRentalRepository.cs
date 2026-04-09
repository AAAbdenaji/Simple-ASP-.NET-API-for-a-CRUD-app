using RentaCarAPI.Models;

namespace RentaCarAPI.Interfaces
{
    public interface IRentalRepository
    {
        ICollection<rental> GetRentals();
        ICollection<rental> GetRentalsOfClient(Guid Clientid);
        bool rentalExists(Guid Clientid, int vehid, DateTime DoA);
        ICollection<rental> GetRentalsOfVehicles(int VehId);

    }
}
