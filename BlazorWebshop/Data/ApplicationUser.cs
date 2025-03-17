using Microsoft.AspNetCore.Identity;

namespace BlazorWebshop.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? Prefix { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? Zipcode { get; set; }
        public string? City { get; set; }
    }

}
