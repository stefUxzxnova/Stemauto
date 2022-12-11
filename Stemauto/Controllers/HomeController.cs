using Microsoft.AspNetCore.Mvc;
using Stemauto.Extentions;
using Stemauto.Entities;
using Stemauto.Repository;
using Stemoblie.ViewModels;
using System.Diagnostics;
using Stemauto.ActionFilters;
using Stemauto.ViewModels.Home;

namespace Stemauto.Controllers
{
    
    public class HomeController : Controller
    {
        [HttpGet]   
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            MyDbContext context = new MyDbContext();
            User loggedUser = context.Users.Where(u => u.Username == model.Username &&
                                     u.Password == model.Password)
                                     .FirstOrDefault();

            if (loggedUser == null)
            {
                this.ModelState.AddModelError("authError", "Invalid username or password!");
                return View(model);
            }

            HttpContext.Session.SetObject<User>("loggedUser", loggedUser);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            

            User item = new User();
            //Намира максималното id и добавя 1 за новото Id На новия user
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
            item.Email = model.Email;
            item.Role = null;

            MyDbContext context = new MyDbContext();

            //add a new user in the UserRepository
            context.Users.Add(item);
            context.SaveChanges();
            return RedirectToAction("Login", "Home");
        }

        [AuthenticationFilterAtribute]
        [HttpGet]
        public IActionResult EditAccount(int id)
        {
            MyDbContext context = new MyDbContext();
            User user = context.Users.Where(x => x.Id == id)
                                     .FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            EditAccountVM model = new EditAccountVM();
            model.Id = user.Id;
            model.Username = user.Username;
            model.Password = user.Password;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            return View(model);
        }

        [AuthenticationFilterAtribute]
        [HttpPost]
        public IActionResult EditAccount(EditAccountVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User item = new User();

            item.Id = model.Id;
            item.Username = model.Username;
            item.Password = model.Password;
            item.FirstName = model.FirstName;
            item.LastName = model.LastName;
            item.Email = model.Email;

            MyDbContext context = new MyDbContext();

            //add a new user in the UserRepository
            context.Users.Update(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [AuthenticationFilterAtribute]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            MyDbContext context = new MyDbContext();

            
            User item = new User();
            item.Id = id;

            //траква се само по primary key
            context.Users.Remove(item);
            context.SaveChanges();

            HttpContext.Session.SetObject<User>("loggedUser", null);

            return RedirectToAction("Index", "Home");
        }

        [AuthenticationFilterAtribute]
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.SetObject<User>("loggedUser", null);

            return RedirectToAction("Index", "Home");
        }
        

    }
}