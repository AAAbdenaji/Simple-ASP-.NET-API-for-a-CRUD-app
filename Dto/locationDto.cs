namespace RentaCarAPI.Dto
{
    public class locationDto
    {
        public int LocId { get; set; }
        public string LocName { get; set; }
    }
    public class LocationCreateDto
    {
        public string LocName { get; set; }
    }
    public class LocationUpdateDto
    {
        public int LocId { get; set; }
        public string LocName { get; set; }
    }
}
