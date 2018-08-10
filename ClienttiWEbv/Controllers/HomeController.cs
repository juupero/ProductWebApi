using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClienttiWEbv.Models;

namespace ClienttiWEbv.Controllers
{

    public class HomeController : Controller
    {
        public int kakka = 0;

        [HttpPost]
        public ActionResult Seuraava(string Pain)
        {
            ++kakka;
            return View("About");
        }
       
        public IActionResult Index()
        {
            return View();
        }

        public async Task< IActionResult> About()
        {
            var client = new MyNamespace.Client("http://kuupi.azurewebsites.net");
            var products = await client.ApiProductsGetProductsGetAsync(kakka, 10);
         
            return View(products);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
