using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiBev.Domain.Models
{
    public class UserRegistration
    {
        public string? UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class JwtSettings
    {
        public required string Secret { get; set; }
        public required string Issuer { get; set; }
        public required string[] Audiences { get; set; }
    }
}
