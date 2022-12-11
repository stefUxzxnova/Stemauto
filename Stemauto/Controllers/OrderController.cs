using Microsoft.AspNetCore.Mvc;
using Stemauto.ActionFilters;
using Stemauto.Entities;
using Stemauto.Extentions;
using Stemauto.Repository;
using Stemauto.ViewModels.Orders;

namespace Stemauto.Controllers
{        
    [AuthenticationFilterAtribute]
    public class OrderController : Controller
    {
        [HttpGet]
        public IActionResult Order(int itemId)
        {
            //създаваме си нова инстанция от order
            MyDbContext context = new MyDbContext();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            //търсим service, чието Id да е равно да itemId, което ни идва през линка
            Service service = context.Services.Where(x => x.Id == itemId).FirstOrDefault();

            //ако няма такъв service ни редиректва
            if (service == null)
            {
                return RedirectToAction("Index", "Services");
            }

            //търсим запис от Orders, който да е един и същ с този който искаме да създадем, т.е.
            //в контекста на loggedUser съответния service да е поръчван 
            
            Order order = context.Orders.Where(o => o.ServiceItemId == itemId
                                                && o.CustomerId == loggedUser.Id
                                                && o.OwnerId == service.OwnerId
                                                && o.Status == Entities.Order.OrderStatus.inProgress)
                                        .FirstOrDefault();
            //ако не е поръчван си добавяме нов в базата
            if (order == null)
            {
                order = new Order();
                order.CustomerId = loggedUser.Id;
                order.ServiceItemId = itemId;
                order.OwnerId = service.OwnerId;
                order.ServiceItemPrice = service.Price;
                order.Date = DateTime.Now;
                order.Quantity = 1;
                order.Status = Entities.Order.OrderStatus.inProgress;
                order.TotalPrice = service.Price * order.Quantity;
                context.Orders.Add(order);
            }
            //ако е => обновяваме количеството и датата
            else
            {
                order.Quantity++;
                order.Date = DateTime.Now;
                order.TotalPrice = service.Price * order.Quantity;
                context.Orders.Update(order);
             
            }

            context.SaveChanges();
            return RedirectToAction("Basket", "Order");

        }

        [HttpGet]
        public IActionResult Basket()
        {
            MyDbContext context = new MyDbContext();
            BasketVM model = new BasketVM();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            model.Orders = context.Orders
                                        .Where(o => o.CustomerId == loggedUser.Id
                                        && o.Status == Entities.Order.OrderStatus.inProgress)
                                        .ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Basket(BasketVM model)
        {

            MyDbContext context = new MyDbContext();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            return View(model);
        }

        //[HttpPost]
        public IActionResult SubmitOrder()
        {

            MyDbContext context = new MyDbContext();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            List<Order> orders = context.Orders.Where(o => o.CustomerId == loggedUser.Id
                                                        && o.Status == Entities.Order.OrderStatus.inProgress)
                                                .ToList();
            foreach (Order order in orders)
            {
                order.Status = Entities.Order.OrderStatus.submitted;
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Sale");
        }

        [HttpGet]
        public IActionResult OrderedItems()
        {
            MyDbContext context = new MyDbContext();
            OrderedItemsVM model = new OrderedItemsVM();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            model.Orders = context.Orders
                                        .Where(o => o.OwnerId == loggedUser.Id
                                        && o.Status == Entities.Order.OrderStatus.submitted)
                                        .ToList();

            return View(model);
        }

        public IActionResult OrderedItemsDone()
        {

            MyDbContext context = new MyDbContext();
            User loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");

            List<Order> orders = context.Orders
                                         .Where(o => o.OwnerId == loggedUser.Id
                                         && o.Status == Entities.Order.OrderStatus.submitted)
                                         .ToList();

            foreach (Order order in orders)
            {
                order.Status = Entities.Order.OrderStatus.done;
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Sale");
        }
    }
}
