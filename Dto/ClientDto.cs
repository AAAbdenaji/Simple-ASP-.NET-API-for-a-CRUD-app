
using RentaCarAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace RentaCarAPI.Dto
{
    public class ClientDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public locationDto Location { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ImagePath { get; set; }
    }
    public class ClientIncludeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public locationDto Location { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ImagePath { get; set; }
    }
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public int LocId { get; set; }
    }
    public class NewClientDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
