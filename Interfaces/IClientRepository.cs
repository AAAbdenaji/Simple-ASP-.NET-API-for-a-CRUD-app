using RentaCarAPI.Models;

namespace RentaCarAPI.Interfaces
{
    public interface IClientRepository
    {
        ICollection<Client> GetClients();
        Client GetClient(Guid Clientid);
        bool ClientExists(Guid Clientid);
        float GetClientTotalPaid(Guid Clientid);
    }
}
