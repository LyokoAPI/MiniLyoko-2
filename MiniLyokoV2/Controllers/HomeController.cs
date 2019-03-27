using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniLyokoV2.Models;

namespace MiniLyokoV2.Controllers
{
    public class HomeController : Controller
    {

        private static Backend.Backend _backend;

        
        public HomeController()
        {
            
        }
        private static async Task initializeBackend()
        {
            await Task.Delay(5000);
            _backend = Backend.Backend.Initialize(Program.Port);
        }

       
        public IActionResult Index()
        {
            initializeBackend();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}