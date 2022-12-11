using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemauto.Entities
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }

        public string Brand { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public int HorsePower { get; set; }
        public int Km { get; set; }
        public int Price { get; set; }
        public string Img { get; set; }


        [ForeignKey(nameof(OwnerId))]
        public virtual User User { get; set; }
    }
}
