using Microsoft.AspNetCore.Mvc;
using Stemauto.ActionFilters;
using Stemauto.Entities;
using Stemauto.Repository;
using Stemauto.ViewModels.Users;
using System.Linq.Expressions;

namespace Stemauto.Controllers
{
	[AuthenticationFilterAtribute]
    public class UsersController : Controller
    {
		[HttpGet]
        public IActionResult Index(IndexVM model)
        {
			MyDbContext context = new MyDbContext();
			

			model.Page = model.Page <= 0 ? 1 : model.Page;
			model.ItemsPerPage = 6;

			Expression<Func<User, bool>> filter = u =>
												(String.IsNullOrEmpty(model.Username) || u.Username.Contains(model.Username)) &&
												(String.IsNullOrEmpty(model.FirstName) || u.FirstName.Contains(model.FirstName)) &&
												(String.IsNullOrEmpty(model.LastName) || u.LastName.Contains(model.LastName));


			model.PagesCount = (int)Math.Ceiling(
									context.Users.Where(filter).Count() / (double)model.ItemsPerPage
									);

			model.Items = context.Users
									.Where(filter)
									.Skip((model.Page - 1) * model.ItemsPerPage)
									.Take(model.ItemsPerPage)
									.ToList();//филтриране на users


			
			return View(model);
		}

        [HttpGet]
        public IActionResult Edit(int id)
        {
            MyDbContext context = new MyDbContext();
            User user = context.Users.Where(x => x.Id == id)
                                     .FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            EditVM model = new EditVM();
            model.Id = user.Id;
            model.Username = user.Username;
            model.Password = user.Password;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
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
            item.Role = model.Role;

            MyDbContext context = new MyDbContext();

            //add a new user in the UserRepository
            context.Users.Update(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            MyDbContext context = new MyDbContext();

            User item = new User();
            item.Id = id;

            context.Users.Remove(item);
            context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

    }
}
