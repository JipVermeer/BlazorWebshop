using System.ComponentModel.DataAnnotations;

namespace Webshop.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een voornaam in")]
        public string? FirstName { get; set; }

        public string? Prefix { get; set; } 

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een achternaam in")]
        public string? LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een straatnaam in")]
        public string? Street { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een huisnummer in")]
        public string? HouseNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een postcode in")]
        public string? Zipcode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een stad in")]
        public string? City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een emailadres in")]
        [EmailAddress(ErrorMessage = "Vul een geldig emailadres in")]
        public string? UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een wachtwoord in")]
        [MinLength(8, ErrorMessage = "Het wachtwoord moet minimaal 8 tekens lang zijn")]
        [RegularExpression(@"^(?=.*[\W_]).{8,}$", ErrorMessage = "Het wachtwoord moet minimaal 8 karakters bevatten, inclusief 1 speciaal teken")]
        public string? Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Herhaal het wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen")]
        public string? PasswordCheck { get; set; }
    }
}

