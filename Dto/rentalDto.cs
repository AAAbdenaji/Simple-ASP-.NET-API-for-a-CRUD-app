using RentaCarAPI.Models;

namespace RentaCarAPI.Dto
{
    public class rentalDto
    {
        public int ClientId { get; set; }
        public int VehId { get; set; }
        public ClientIncludeDto Client { get; set; }
        public VehicleIncludeDto Vehicle { get; set; }
        public DateTime DateOfRental { get; set; }
        public int DurationDays { get; set; }
        public float TotalPrice { get; set; }
        public DateTime EndOfRent { get; set; }
    }
}
