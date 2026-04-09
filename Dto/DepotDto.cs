using RentaCarAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentaCarAPI.Dto
{
    public class DepotDto
    {
        public int DepId { get; set; }
        public string DepName { get; set; }
        public locationDto Location { get; set; }
        public string ImagePath { get; set; }
    }
    public class DepotVehCreateDto
    {
        public int DepotId { get; set; }
    }
    public class DepotCreateDto
    {
        public string DepName { get; set; }
        public int LocId { get; set; }
    }
    public class DepotUpdateDto
    {
        public int DepId { get; set; }
        public string DepName { get; set; }
    }
}
