using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaCarAPI.Models
{
    public class Depot
    {
        [Key]
        public int DepId { get; set; }
        public string DepName { get; set; }
        public Location Location { get; set; }
        public int LocId { get; set; }
        [ForeignKey("LocId")]
        public ICollection<Vehicle> vehicles { get; set; }
        public string ImagePath { get; set; }
    }
}
