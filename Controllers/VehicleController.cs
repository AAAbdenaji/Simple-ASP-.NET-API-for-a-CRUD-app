using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RentaCarAPI.Dto;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;
using RentaCarAPI.Repository;

namespace RentaCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VehicleController(IVehicleRepository vehicleRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;   
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = _mapper.Map<List<VehicleDto>>(await _vehicleRepository.GetVehicles());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Response.Headers.Add("Content-Type", "application/json");
            return Ok(vehicles);
        }
        [HttpGet("{VehId}")]
        [ProducesResponseType(200,Type = typeof(Vehicle))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVehicle(int VehId)
        {
            if (!await _vehicleRepository.VehicleExists(VehId))
                return NotFound();
            var vehicle = _mapper.Map<VehicleDto>(await _vehicleRepository.GetVehicle(VehId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(vehicle);
        }
        [HttpGet("{VehId}/days")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVehicleRentedDays(int VehId)
        {
            if (!await _vehicleRepository.VehicleExists(VehId))
                return NotFound();
            var days = await _vehicleRepository.GetVehicleRentedDays(VehId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(days);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVehicle([FromForm] VehicleCreateDto VehicleCreate, IFormFile img)
        {
            if (VehicleCreate == null)
                return BadRequest(ModelState);
            if (img == null)
                return BadRequest("Null Image");
            if (!IsFileExtensionAllowed(img, new string[] { ".jpg", ".jpeg", ".png",".jfif" }))
                return BadRequest("invalid extension. only jpg jpeg and png formats are supported currently");
            if (!IsFileSizeWithinLimit(img, (1024 * 1024) * 10))
                return BadRequest("file exceeds maximum size of 10 MBs");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + img.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            var VehicleMap = _mapper.Map<Vehicle>(VehicleCreate);

            VehicleMap.ImgPath = "/Images/" + uniqueFileName;
            VehicleMap.IsRented = false;
            VehicleMap.DateOfArrival = DateTime.Now;

            if (!await _vehicleRepository.CreateVehicle(VehicleMap))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500, ModelState);
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }
            return Ok("Successfully Created");
        }
        [HttpGet("vehicle-img/{VehId}")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetVehicleImage(int VehId)
        {

            if (!await _vehicleRepository.VehicleExists(VehId))
                return NotFound();

            var vehicle = await _vehicleRepository.GetVehicle(VehId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string fullImagePath = GetFullImagePath(vehicle.ImgPath);
            var image = System.IO.File.OpenRead(fullImagePath);
            return File(image, "image/jpeg");
        }
        [HttpPut("{VehId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateVehicle(int VehId, [FromForm] VehicleUpdateDto UpdatedVehicle)
        {
            if (UpdatedVehicle == null)
                return BadRequest(ModelState);
            if (VehId != UpdatedVehicle.VehId)
                return BadRequest(ModelState);
            if (!await _vehicleRepository.VehicleExists(VehId))
                return NotFound();
            var vehicle = await _vehicleRepository.GetVehicle(VehId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            vehicle.DepId = UpdatedVehicle.DepId;
            vehicle.Color = UpdatedVehicle.Color;
            vehicle.Brand = UpdatedVehicle.Brand;
            vehicle.Model = UpdatedVehicle.Model;
            vehicle.IsRented = UpdatedVehicle.IsRented;
            vehicle.PricePerDay = UpdatedVehicle.PricePerDay;

            if (!_vehicleRepository.UpdateVehicle(vehicle))
            {
                ModelState.AddModelError("", "Something Went Wrong updating Vehicle");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        private string GetFullImagePath(string relativePath)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));
        }
        public static bool IsFileExtensionAllowed(IFormFile file, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(file.FileName);
            return allowedExtensions.Contains(extension);
        }
        public static bool IsFileSizeWithinLimit(IFormFile file, long maxSizeInBytes)
        {
            return file.Length <= maxSizeInBytes;
        }
    }
}
