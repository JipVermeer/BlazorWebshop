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
        public string? FirstName { get; set; }
        public string? Prefix { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }
    }
}
