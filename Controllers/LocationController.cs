using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentaCarAPI.Dto;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;
using RentaCarAPI.Repository;
using System.Drawing.Text;

namespace RentaCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationController(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = _mapper.Map<List<locationDto>>(await _locationRepository.GetLocations());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(locations);
        }
        [HttpGet("{LocId}")]
        public async Task<IActionResult> GetLocation(int LocId)
        {
            if (!await _locationRepository.LocationExists(LocId))
                return NotFound();
            var location = _mapper.Map<locationDto>(await _locationRepository.GetLocation(LocId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(location);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLocation([FromForm] LocationCreateDto locationCreate)
        {
            if (locationCreate == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var LocationMap = _mapper.Map<Location>(locationCreate);

            if(!await _locationRepository.CreateLocation(LocationMap))
            {
                ModelState.AddModelError("", "Something Went Wrong");
                return StatusCode(500,ModelState);
            }
            return Ok("Location Successfully Created");
        }
        [HttpPut("{LocId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLocation (int LocId, [FromForm] LocationUpdateDto updatedLocation)
        {
            if (updatedLocation == null)
                return BadRequest(ModelState);
            if (LocId != updatedLocation.LocId)
                return BadRequest(ModelState);
            if (!await _locationRepository.LocationExists(LocId))
                return NotFound();
            var location = await _locationRepository.GetLocation(LocId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            location.LocName = updatedLocation.LocName;

            if(!await _locationRepository.UpdateLocation(location))
            {
                ModelState.AddModelError("", "Something Went Wrong updating Vehicle");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
