using System.Diagnostics;
using Capitulo01.Modelo.Cadastros;
using Microsoft.AspNetCore.Mvc;
using Capitulo01.Models;

namespace Capitulo01.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 404 || statusCode == 500)
                {
                    var viewName = $"Erro{statusCode.ToString()}";
                    return View(viewName);
                }
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}