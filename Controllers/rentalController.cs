using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentaCarAPI.Data;
using RentaCarAPI.Dto;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;

namespace RentaCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class rentalController : Controller
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public rentalController(IRentalRepository rentalRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<rental>))]
        public IActionResult GetRentals()
        {
            var rentals = _mapper.Map<List<rentalDto>>(_rentalRepository.GetRentals());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(rentals);
        }

    }
}
