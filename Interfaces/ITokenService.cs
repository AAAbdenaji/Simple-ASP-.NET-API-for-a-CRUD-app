using RentaCarAPI.Models;

namespace RentaCarAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(Client client);
    }
}
