using Practica__3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica__3.Models;

namespace Practica__3.Controllers
{
    public class ComprasController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IUtilitarios _util;

        public ComprasController(IHttpClientFactory http, IUtilitarios util)
        {
            _http = http;
            _util = util;
        }

      

        public async Task<IActionResult> Consulta()
        {
            try
            {
                var api = _http.CreateClient("Api");
                var data = await api.GetFromJsonAsync<IEnumerable<Compra>>("api/compras");
                return View(data ?? Enumerable.Empty<Compra>());
            }
            catch (Exception ex)
            {
                return Content("Fallo llamando al API: " + ex.Message);
            }
        }
    }
}