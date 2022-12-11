using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Stemauto.ViewModels.Sale
{
	public class AskVM
	{
        public int Id { get; set; }

        [DisplayName("First Name: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string FirstName { get; set; }

        [DisplayName("Last Name: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string LastName { get; set; }

        [DisplayName("Subject: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Subject { get; set; }

        [DisplayName("Question: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public string Body { get; set; }
    }
}
