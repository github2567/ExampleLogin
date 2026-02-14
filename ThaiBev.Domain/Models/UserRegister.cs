using System.ComponentModel.DataAnnotations;

namespace ThaiBev.Domain.Models
{
    public class UserRegister
    {
        public string? UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
