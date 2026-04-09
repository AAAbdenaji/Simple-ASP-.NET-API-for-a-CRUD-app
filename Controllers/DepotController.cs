using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentaCarAPI.Dto;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;
using RentaCarAPI.Repository;
using System;

namespace RentaCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepotController : ControllerBase
    {
        private readonly IDepotRepository _depotRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DepotController(IDepotRepository depotRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _depotRepository = depotRepository;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetDepots()
        {
            var depots = _mapper.Map<List<DepotDto>>(await _depotRepository.GetDepots());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(depots);
        }
        [HttpGet("{DepId}")]
        public async Task<IActionResult> GetDepot(int DepId)
        {
            if (!await _depotRepository.DepotExists(DepId))
                return NotFound();
            var depot = _mapper.Map<DepotDto>(await _depotRepository.GetDepot(DepId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(depot);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDepot([FromForm] DepotCreateDto depotCreate, IFormFile img)
        {
            if (depotCreate == null)
                return BadRequest(ModelState);
            if (img == null)
                return BadRequest("Null Image");
            if (!IsFileExtensionAllowed(img, new string[] { ".jpg", ".jpeg", ".png", ".jfif" }))
                return BadRequest("invalid extension. only jpg jpeg jfif and png formats are supported currently");
            if (!IsFileSizeWithinLimit(img, (1024 * 1024) * 10))
                return BadRequest("file exceeds maximum size of 10 MBs");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + img.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            var DepotMap = _mapper.Map<Depot>(depotCreate);

            DepotMap.ImagePath = "/images/" + uniqueFileName;

            if (!await _depotRepository.CreateDepot(DepotMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }
            return Ok("Sucessfully Created");
        }
        [HttpGet("depot-img/{DepId}")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDepotImg(int DepId)
        {
            if (!await _depotRepository.DepotExists(DepId))
                return NotFound();
            var depot = await _depotRepository.GetDepot(DepId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string fullimgPath = GetFullImagePath(depot.ImagePath);
            var image = System.IO.File.OpenRead(fullimgPath);
            return File(image, "image/jpeg");

        }
        [HttpPut("{DepId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepot(int DepId,[FromForm] DepotUpdateDto depotUpdate)
        {
            if (depotUpdate == null)
                return BadRequest(depotUpdate);
            if (DepId != depotUpdate.DepId)
                return BadRequest(ModelState);
            if (!await _depotRepository.DepotExists(DepId))
                return NotFound();
            var depot = await _depotRepository.GetDepot(DepId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            depot.DepName = depotUpdate.DepName;

            if(!await _depotRepository.UpdateDepot(depot))
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

