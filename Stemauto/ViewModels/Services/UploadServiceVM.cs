using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Stemauto.ViewModels.Services
{
    public class UploadServiceVM
    {
        [DisplayName("Title: ")]
        [Required(ErrorMessage = "This field is Required!")]

        public string Title { get; set; }
        [DisplayName("Description: ")]
        [Required(ErrorMessage = "This field is Required!")]

        public string Description { get; set; }

        
        [DisplayName("Price: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public int Price { get; set; }


    }
}
