using Microsoft.AspNetCore.Mvc;
using OrderBookWebApp.Models;
using System.Diagnostics;

namespace OrderBookWebApp.Controllers
{
    /// <summary>
    /// The main application's contoller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The main page with order book interface
        /// </summary>
        /// <returns>The view with order book interface</returns>
        public IActionResult Index()
            => View();

        /// <summary>
        /// The error page with description
        /// </summary>
        /// <returns>The view with information about error</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
