using Stemauto.Entities;

namespace Stemauto.ViewModels.Orders
{
    public class BasketVM
    {
        public int Id { get; set; } 
        public List<Order> Orders { get; set; }
        public int Quantity { get; set; }

    }
}
