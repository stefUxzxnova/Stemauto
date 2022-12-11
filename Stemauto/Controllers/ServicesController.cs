using Microsoft.AspNetCore.Mvc;
using Stemauto.ActionFilters;
using Stemauto.Entities;
using Stemauto.Extentions;
using Stemauto.Repository;
using Stemauto.ViewModels.Services;

namespace Stemauto.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index(IndexVM model)
        {
            MyDbContext context = new MyDbContext();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if(loggedUser == null)
			{
                model.Services = context.Services
                                         .ToList();
			}
			else
			{
                model.Services = context.Services
                                    .Where(p => p.OwnerId != loggedUser.Id)
                                    .ToList();
			}

            
            return View(model);
        }

        [AuthenticationFilterAtribute]
        [HttpGet]
        public IActionResult UploadService()
        {
            return View();
        }

        [AuthenticationFilterAtribute]
        [HttpPost]
        public IActionResult UploadService(IFormCollection collection, UploadServiceVM model)
        {
            MyDbContext context = new MyDbContext();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Service item = new Service();

            //къде ще запазваме снимките
            string path = @"C:\Users\User\OneDrive\Desktop\Stemauto\Stemauto\Stemauto\wwwroot\Resourses\ServicesPhotos\";

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
            item.Title = model.Title;
            item.Description = model.Description;
            item.Price = model.Price;

            context.Services.Add(item);
            context.SaveChanges();
            return RedirectToAction("MyServices", "Services");
        }


        [AuthenticationFilterAtribute]
        [HttpGet]
        public IActionResult MyServices(MyServicesVM model)
        {
            MyDbContext context = new MyDbContext();

            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            model.Services = context.Services
                                    .Where(p => p.OwnerId == loggedUser.Id)
                                    .ToList();
            return View(model);
        }

        [AuthenticationFilterAtribute]
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            EditMyServiceVM model = new EditMyServiceVM();

            MyDbContext context = new MyDbContext();

            Service item = context.Services.Where(p => p.Id == Id).FirstOrDefault();

            if (item == null)
            {
                return RedirectToAction("Index", "Services");
            }


            model.Id = item.Id;
            model.OwnerId = item.OwnerId;
            model.Img = item.Img;

            model.Title = item.Title;
            model.Description = item.Description;
            model.Price = item.Price;

            return View(model);
        }

        [AuthenticationFilterAtribute]
        [HttpPost]
        public IActionResult Edit(IFormCollection collection, EditMyServiceVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Services");
            }
            //User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");


            MyDbContext context = new MyDbContext();

            Service item = new Service();

            string path = @"C:\Users\User\OneDrive\Desktop\Stemauto\Stemauto\Stemauto\wwwroot\Resourses\ServicesPhotos\";
            if (this.Request.Form.Files.Count > 0 && this.Request.Form.Files[0].Length > 0 && this.Request.Form.Files[0].FileName != model.Img)
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
                item.Img = model.Img;
            }


            item.Id = model.Id;
            item.OwnerId = model.OwnerId;
            item.Title = model.Title;
            item.Description = model.Description;
            item.Price = model.Price;

            context.Services.Update(item);
            context.SaveChanges();

            return RedirectToAction("MyServices", "Services");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            MyDbContext context = new MyDbContext();

            Service item = new Service();
            item.Id = id;

            context.Services.Remove(item);
            context.SaveChanges();

            return RedirectToAction("MyServices", "Services");
        }


       
    }
}
