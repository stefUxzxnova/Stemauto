using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stemauto.Entities
{
    public class Order
    {
        public enum OrderStatus { 
            inProgress,
            submitted,
            done
        }


        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceItemId { get; set; }
        public int Quantity { get; set; }
        public int ServiceItemPrice { get; set; }
        public int TotalPrice { get; set; }
        public OrderStatus Status { get; set; }


        public DateTime Date { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual User Customer { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual User Owner { get; set; }
        
        [ForeignKey(nameof(ServiceItemId))]
        public virtual Service Item { get; set; }

       

    }
}
