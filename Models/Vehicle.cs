using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaCarAPI.Models
{
    public class Vehicle
    {
        [Key]
        public int VehId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public DateTime DateOfArrival { get; set; }
        public Depot Depot { get; set; }
        public int DepId { get; set; }
        [ForeignKey("DepId")]
        public bool IsRented { get; set; }
        public float PricePerDay { get; set; }
        public string ImgPath { get; set; }
        public ICollection<rental> rentals { get; set; }
    }
}
