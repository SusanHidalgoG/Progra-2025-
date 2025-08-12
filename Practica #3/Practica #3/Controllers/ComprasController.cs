using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica__3.Models;

namespace Practica__3.Controllers
{
    public class ComprasController : Controller
    {
        private readonly IHttpClientFactory _http;
        public ComprasController(IHttpClientFactory http) => _http = http;

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
                TempData["Error"] = "No se pudo conectar con el API: " + ex.Message;
                return View(Enumerable.Empty<Compra>());
            }
        }
    }
}