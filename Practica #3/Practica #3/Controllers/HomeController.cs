using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Practica__3.Models;

namespace Practica__3.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _http;
        public HomeController(IConfiguration configuration, IHttpClientFactory http)
        {
            _configuration = configuration;
            _http = http;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        [Route("Home/Error")]
        public IActionResult Error() => View("~/Views/Shared/Error.cshtml");
    }
}
