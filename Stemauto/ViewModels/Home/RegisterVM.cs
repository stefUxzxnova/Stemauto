using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Stemauto.ViewModels.Home
{
    public class RegisterVM
    {

        [DisplayName("Username: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Username { get; set; }

        [DisplayName("First Name: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string FirstName { get; set; }

        [DisplayName("Last Name: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string LastName { get; set; }

        [DisplayName("Password: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Password { get; set; }

        [DisplayName("Email: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ComparePassword { get; set; }


    }
} 
