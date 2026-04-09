using RentaCarAPI.Models;

namespace RentaCarAPI.Dto
{
    public class VehicleDto
    {
        public int VehId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public DateTime DateOfArrival { get; set; }
        public DepotDto Depot { get; set; }
        public int DepId { get; set; }
        public bool IsRented { get; set; }
        public float PricePerDay { get; set; }
        public string ImgPath { get; set; }
    }
    public class VehicleIncludeDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public DateTime DateOfArrival { get; set; }
        public DepotDto Depot { get; set; }
        public int DepId { get; set; }
        public bool IsRented { get; set; }
        public float PricePerDay { get; set; }
        public string ImgPath { get; set; }
    }
    public class VehicleCreateDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int DepId { get; set; }
        public float PricePerDay { get; set; }
    }
    public class VehicleUpdateDto
    {
        
        public int VehId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int DepId { get; set; }
        public bool IsRented { get; set; }
        public float PricePerDay { get; set; }
    }

}
