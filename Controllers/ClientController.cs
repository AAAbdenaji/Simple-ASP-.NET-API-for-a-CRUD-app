using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentaCarAPI.Dto;
using RentaCarAPI.Interfaces;
using RentaCarAPI.Models;
using RentaCarAPI.Repository;

namespace RentaCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Client> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<Client> _signInManager;
        public ClientController(IClientRepository clientRepository,IMapper mapper, UserManager<Client> userManager, IWebHostEnvironment webHostEnvironment, ITokenService tokenService,SignInManager<Client> signInManager)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;   
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("MakaAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeAdmin(Guid ClientId)
        {
            var client = _clientRepository.GetClient(ClientId);
            if (client == null)
                return NotFound();
            var roleresult = await _userManager.AddToRoleAsync(client, "Admin");
            if (roleresult.Succeeded)
            {
                return Ok("Done.");
            }
            else
            {
                return BadRequest(roleresult.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var client = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (client == null) return Unauthorized("Invalid username!");
            var result = await _signInManager.CheckPasswordSignInAsync(client, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");
            return Ok(
                new NewClientDto
                {
                    Username = client.UserName,
                    Email = client.Email,
                    Token = await _tokenService.CreateToken(client)
                }
                ); ;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto, IFormFile img)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + img.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                

            var AClient = _mapper.Map<Client>(registerDto);
            AClient.ImagePath = "/images/" + uniqueFileName;
            AClient.RegistrationDate = DateTime.Now;

                
            var createdUser = await _userManager.CreateAsync(AClient, registerDto.Password);
            if (!createdUser.Succeeded)
                return StatusCode(500, createdUser.Errors);
    
            var roleResult = await _userManager.AddToRoleAsync(AClient, "User");
            if (!roleResult.Succeeded)
                return StatusCode(500, roleResult.Errors);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }
            return Ok(
                    new NewClientDto
                    {
                        Username = AClient.UserName,
                        Email = AClient.Email,
                        Token = await _tokenService.CreateToken(AClient)
                });

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Vehicle>))]
        public IActionResult GetClients()
        {
            var clients = _mapper.Map<List<ClientDto>>(_clientRepository.GetClients());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(clients);
        }
        [HttpGet("{ClientId}")]
        [ProducesResponseType(200, Type = typeof(Vehicle))]
        [ProducesResponseType(400)]
        [Authorize(Roles = "Admin")]
        public IActionResult GetClient(Guid ClientId)
        {
            if (!_clientRepository.ClientExists(ClientId))
                return NotFound();
            var client = _mapper.Map<ClientDto>(_clientRepository.GetClient(ClientId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(client);
        }
        [HttpGet("{ClientId}/totalpaid")]
        [ProducesResponseType(200, Type = typeof(float))]
        [ProducesResponseType(400)]
        public IActionResult GetClientTotalPaid(Guid ClientId)
        {
            if (!_clientRepository.ClientExists(ClientId))
                return NotFound();
            var totalpaid = _mapper.Map<float>(_clientRepository.GetClientTotalPaid(ClientId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(totalpaid);
        }
        [HttpGet("{ClientId}/img")]
        public async Task<IActionResult> GetClientImg(Guid ClientId)
        {
            var client = _clientRepository.GetClient(ClientId);
            if (client == null)
                return NotFound();
            string imgFullpath = GetFullImagePath(client.ImagePath);
            var image = System.IO.File.OpenRead(imgFullpath);

            return File(image, "image/jpeg");
        }
        private string GetFullImagePath(string relativePath)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));
        }
    }
}
