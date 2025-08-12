using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Practica__3.Models;
using Practica__3.Services;

namespace Practica__3.Controllers
{
    public class AbonosController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IUtilitarios _util;

        public AbonosController(IHttpClientFactory http, IUtilitarios util)
        {
            _http = http;
            _util = util;
        }

        // GET: /Abonos/Registro
        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            var api = _http.CreateClient("Api");
            IEnumerable<CompraPend> pendientes = Enumerable.Empty<CompraPend>();

            try
            {
                pendientes = await api.GetFromJsonAsync<IEnumerable<CompraPend>>("api/compras/pendientes")
                             ?? Enumerable.Empty<CompraPend>();
            }
            catch
            {
                TempData["Error"] = "No se pudo cargar el listado de compras pendientes. ¿Está corriendo el API?";
            }

            return View(pendientes);
        }

        [HttpPost]
        public async Task<IActionResult> Registro(AbonoRequest model)
        {
    
            if (model == null || model.Id_Compra <= 0 || model.Monto <= 0)
            {
                ViewBag.Error = "Complete los datos del abono.";
                var api0 = _http.CreateClient("Api");
                var pend0 = await api0.GetFromJsonAsync<IEnumerable<CompraPend>>("api/compras/pendientes")
                           ?? Enumerable.Empty<CompraPend>();
                ViewBag.SelId = model?.Id_Compra;
                ViewBag.Monto = model?.Monto;
                return View(pend0); 
            }

            try
            {
                var api = _http.CreateClient("Api");
                var resp = await api.PostAsJsonAsync("api/Abono", model);

                if (resp.IsSuccessStatusCode)
                    return RedirectToAction("Consulta", "Compras");

                var body = await resp.Content.ReadAsStringAsync();
                ViewBag.Error = string.IsNullOrWhiteSpace(body)
                    ? $"Error al registrar el abono (código {(int)resp.StatusCode})."
                    : body;

                var pend = await api.GetFromJsonAsync<IEnumerable<CompraPend>>("api/compras/pendientes")
                           ?? Enumerable.Empty<CompraPend>();
                ViewBag.SelId = model.Id_Compra;
                ViewBag.Monto = model.Monto;
                return View(pend);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Excepción en POST /Abonos/Registro: " + ex.Message;
                var api = _http.CreateClient("Api");
                var pend = await api.GetFromJsonAsync<IEnumerable<CompraPend>>("api/compras/pendientes")
                           ?? Enumerable.Empty<CompraPend>();
                ViewBag.SelId = model.Id_Compra;
                ViewBag.Monto = model.Monto;
                return View(pend); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSaldo(long id)
        {
            var api = _http.CreateClient("Api");
            var r = await api.GetAsync($"api/compras/{id}/saldo");
            if (!r.IsSuccessStatusCode) return NotFound();
            var saldo = await r.Content.ReadFromJsonAsync<decimal>();
            return Json(new { saldo });
        }
    }
}
