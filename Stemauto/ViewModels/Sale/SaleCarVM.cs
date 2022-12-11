using Stemauto.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Stemauto.ViewModels.Sale

{
    public class SaleCarVM
    {
        public Brands Brandenum { get; set; }

        [DisplayName("Brand: ")]
        [Required(ErrorMessage = "This field is Required!")]

        public string Brand { get; set; }
        [DisplayName("Description: ")]
        [Required(ErrorMessage = "This field is Required!")]

        public string Description { get; set; }

        [DisplayName("Year: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public int Year { get; set; }

        [DisplayName("HP: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public int HorsePower { get; set; }

        [DisplayName("Km: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public int Km { get; set; }

        [DisplayName("Price: ")]
        [Required(ErrorMessage = "This field is Required!")]
        public int Price { get; set; }

    }
}
