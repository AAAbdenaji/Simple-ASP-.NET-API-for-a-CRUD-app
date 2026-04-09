using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaCarAPI.Models
{
    public class Client : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Location Location { get; set; }
        public int LocId { get; set; }
        [ForeignKey("LocId")]
        public DateTime RegistrationDate { get; set; }
        public string ImagePath { get; set; }
        public ICollection<rental> rentals { get; set; }
    }
}
