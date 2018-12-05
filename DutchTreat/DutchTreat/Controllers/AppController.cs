using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository repo;      
      

        public AppController(IMailService mailService
            , IDutchRepository repo)
        {
            this.mailService = mailService;
            this.repo = repo;
         
        }

        public IActionResult Index()
        {
            var results = repo.GetAllProducts();
            //throw new InvalidOperationException("Bad things happened!");

            return View(results);
        }

        // in felul acesta link-ul catre Contact (ruta) va fi de forma localhost:8888/contact fara a mai fi nevoie sa intercalam si numele controleru-lui "App" 
        //interesant este ca numele din interiorul HttpGet("___") poate fi oricare dar metoda Contact() va fi apelata in spate.
        //Este o metoda de a suprascrie ruta definita in Startup.cs (     app.UseMvc(cfg =>
        //   {
        //       cfg.MapRoute("Default","/{controller}/{action}/{id?}", new { controller = "App",Action = "Index"});
        //    });)
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            // throw new InvalidOperationException("Ceva nasol s-a intamplat");

            ViewBag.Title = "Contact Us";

            return View();
        }
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Send email    
                mailService.SendMessage("gvames@gmail.com", model.Subject, $"From: {model.Name} - {model.Name}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }

            ViewBag.Title = "Contact Us";
            return View();

        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            var test = View();
            return View();
        }

        [Authorize]
        public IActionResult Shop()
        {
            var results = repo.GetAllProducts();
            return View(results.ToList());
        }
    }
}
