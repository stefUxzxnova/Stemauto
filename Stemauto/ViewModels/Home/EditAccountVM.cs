using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Stemauto.ViewModels.Home
{
  
	public class EditAccountVM
	{
		public int Id { get; set; }

		[DisplayName("Username: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Username { get; set; }
		[DisplayName("Password: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Password { get; set; }
		[DisplayName("First name: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string FirstName { get; set; }
		[DisplayName("Last name: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string LastName { get; set; }
		[DisplayName("Email: ")]
		[Required(ErrorMessage = "This field is Required!")]
		public string Email { get; set; }
	}
}
