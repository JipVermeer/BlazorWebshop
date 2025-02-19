using System.ComponentModel.DataAnnotations;

namespace Webshop.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een gebruikersnaam in")]
        public string? UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Vul een wachtwoord in")]
        public string? Password { get; set; }
    }
}
