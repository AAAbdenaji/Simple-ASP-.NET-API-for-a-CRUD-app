using System.ComponentModel.DataAnnotations;

namespace RentaCarAPI.Models
{
    public class Location
    {
        [Key]
        public int LocId { get; set; }
        public string LocName { get; set; }
        public ICollection<Client> Clients { get; set; }
        public ICollection<Depot> Depots { get; set; }
    }
}
