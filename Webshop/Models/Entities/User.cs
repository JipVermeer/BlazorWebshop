using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Webshop.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public String? Role { get; set; }
    }
}
