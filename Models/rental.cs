using System.ComponentModel.DataAnnotations;

namespace RentaCarAPI.Models
{
    public class rental
    {
        public Guid ClientId { get; set; }
        public int VehId { get; set; }
        public Client Client { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime DateOfRental { get; set; }
        public int DurationDays { get; set; }
        public float TotalPrice { get; set; }
        public DateTime EndOfRent { get; set; }

    }
}
