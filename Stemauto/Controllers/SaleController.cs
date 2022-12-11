using Microsoft.AspNetCore.Mvc;
using Stemauto.ActionFilters;
using Stemauto.Entities;
using Stemauto.Extentions;
using Stemauto.Repository;
using Stemauto.ViewModels.Sale;
using Stemauto.ViewModels.Shared;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;

namespace Stemauto.Controllers
{
    public class SaleController : Controller
    {
        public IActionResult Index(IndexVM model)
        {
			MyDbContext context = new MyDbContext();


			model.Pager ??= new PagerVM();

			model.Pager.Page = model.Pager.Page <= 0 ? 1 : model.Pager.Page;
			model.Pager.ItemsPerPage = model.Pager.ItemsPerPage <= 0 ? 10 : model.Pager.ItemsPerPage;

			model.Filter = model.Filter ?? new FilterVM();
			Expression<Func<Car, bool>> filter = u =>
												(String.IsNullOrEmpty(model.Filter.Brand) || u.Brand.Contains(model.Filter.Brand));

			model.Pager.PagesCount = (int)Math.Ceiling(
									context.Cars.Where(filter).Count() / (double)model.Pager.ItemsPerPage
									);


			model.Cars = context.Cars
								.OrderBy(i => i.Id)
								.Where(filter)
								.Skip((model.Pager.Page - 1) * model.Pager.ItemsPerPage)
								.Take(model.Pager.ItemsPerPage)
								.ToList();

			return View(model);
		}

		[AuthenticationFilterAtribute]
		[HttpGet]
		public IActionResult SaleCar()
		{
			return View();
		}

		[AuthenticationFilterAtribute]
		[HttpPost]
		public IActionResult SaleCar(IFormCollection collection, SaleCarVM model)
		{
			MyDbContext context = new MyDbContext();
			User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Car item = new Car();

			//къде ще запазваме снимките
			string path = @"C:\Users\User\OneDrive\Desktop\Stemauto\Stemauto\Stemauto\wwwroot\Resourses\CarsPhotos\";

			if (this.Request.Form.Files.Count > 0 && this.Request.Form.Files[0].Length > 0)
			{
				Stream readStream = this.Request.Form.Files[0].OpenReadStream();
				FileStream writeStream = new FileStream(path + this.Request.Form.Files[0].FileName, FileMode.Create);

				byte[] buffer = new byte[1024];
				while (true)
				{
					int lenght = readStream.Read(buffer, 0, buffer.Length);

					if (lenght == 0)
					{
						break;
					}
					writeStream.Write(buffer, 0, lenght);
				}

				//записваме в базата името на снимката
				item.Img = this.Request.Form.Files[0].FileName;

				readStream.Close();
				writeStream.Close();
			}
			else
			{
				item.Img = "defaultPhoto.jpg";
			}

			item.OwnerId = loggedUser.Id;
			item.Brand = model.Brand;
			item.Description = model.Description;
			item.Year = model.Year;
			item.Price = model.Price;
			item.Km = model.Km;
			item.HorsePower = model.HorsePower;


			context.Cars.Add(item);
			context.SaveChanges();
			return RedirectToAction("Index", "Sale");
		}


		[AuthenticationFilterAtribute]
		[HttpGet]
		public IActionResult MyCars(MyCarsVM model)
		{
			MyDbContext context = new MyDbContext();

			User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

			model.Cars = context.Cars
									.Where(p => p.OwnerId == loggedUser.Id)
									.ToList();
			return View(model);
		}

		[AuthenticationFilterAtribute]
		[HttpGet]
		public IActionResult Edit(int Id)
        {
			EditMyCarVM model = new EditMyCarVM();

			MyDbContext context = new MyDbContext();

			Car car = context.Cars.Where(p => p.Id == Id).FirstOrDefault();

			if (car == null)
			{
				return RedirectToAction("Index", "Sale");
			}


			model.Id = car.Id;
			model.OwnerId = car.OwnerId;
			model.Img = car.Img;
			model.Brand = car.Brand;
			model.Description = car.Description;
			model.HorsePower = car.HorsePower;
			model.Price = car.Price;
			model.Year = car.Year;
			model.Km = car.Km;

			return View(model);
        }

		[AuthenticationFilterAtribute]
		[HttpPost]
		public IActionResult Edit(EditMyCarVM model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Index", "Sale");
			}
			//User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");


			MyDbContext context = new MyDbContext();

			Car car = new Car();

			car.Id = model.Id;
			car.OwnerId = model.OwnerId;
			car.Img = model.Img;
			car.Brand = model.Brand;
			car.Description = model.Description;
			car.HorsePower = model.HorsePower;
			car.Price = model.Price;
			car.Year = model.Year;
			car.Km = model.Km;

			context.Cars.Update(car);
			context.SaveChanges();

			return RedirectToAction("MyCars", "Sale");
		}

		[HttpGet]
		public IActionResult Delete(int id)
		{
			MyDbContext context = new MyDbContext();

			Car item = new Car();
			item.Id = id;

			context.Cars.Remove(item);
			context.SaveChanges();

			return RedirectToAction("MyCars", "Sale");
		}

		[AuthenticationFilterAtribute]
		[HttpGet]
		public IActionResult Ask(int id)
		{	
			MyDbContext context = new MyDbContext();
			Car item = context.Cars
						.Where(x => x.Id == id)
						.FirstOrDefault();

			if (item == null)
			{
				return RedirectToAction("Index", "Sale");
			}
			

			AskVM model = new AskVM();
			model.Subject = item.Brand + " " + item.HorsePower.ToString() +"hp";
			return View(model);
		}


		[HttpPost]
		public IActionResult Ask(AskVM model)
		{
			//if (!ModelState.IsValid)
			//{
			//	return RedirectToAction("Index", "Sale");
			//}
			User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

			if (loggedUser == null)
			{
				return RedirectToAction("Index", "Home");
			}

			MyDbContext context = new MyDbContext();

			Car item = context.Cars.Where(u => u.Id == model.Id)
									.FirstOrDefault();

            if (item == null)
            {
				return RedirectToAction("Index", "Sale");
			}
			model.Subject = item.Brand + " " + item.HorsePower.ToString() + "hp";
			string body = $"{model.FirstName} {model.LastName} is asking... \n {model.Body}";
			try
			{
				var client = new SmtpClient("smtp.gmail.com", 587)
				{
					Credentials = new NetworkCredential("chiche021223@gmail.com", "kiyiywqqonsopkgg"),
					EnableSsl = true
				};

				client.Send(loggedUser.Email,"chiche021223@gmail.com", model.Subject, body);
				Console.WriteLine("Sent");
			}
			catch (Exception)
			{
				ViewData["Message"] = "The email was not send";
			}

			return RedirectToAction("Index", "Sale");
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			DetailsVM model = new DetailsVM();

			MyDbContext context = new MyDbContext();

			Car car = context.Cars.Where(p => p.Id == id).FirstOrDefault();

			if (car == null)
			{
				return RedirectToAction("Index", "Sale");
			}


			model.Id = car.Id;
			model.Img = car.Img;
			model.Brand = car.Brand;
			model.Description = car.Description;
			model.HorsePower = car.HorsePower;
			model.Price = car.Price;
			model.Year = car.Year;
			model.Km = car.Km;
			model.OwnerEmail = car.User.Email;
			
			return View(model);
		}


	}
}
