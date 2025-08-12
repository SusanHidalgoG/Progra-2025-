
using Practica__3.Services;
using Microsoft.AspNetCore.Mvc;
using Practica__3.Models;

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

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            var api = _http.CreateClient("Api");
            var pendientes = await api.GetFromJsonAsync<IEnumerable<CompraPend>>("api/compras/pendientes");
            return View(pendientes ?? Enumerable.Empty<CompraPend>());
        }

        [HttpPost]
        public async Task<IActionResult> Registro(AbonoRegistro vm)
        {
            if (!ModelState.IsValid) return await Registro();

            long idCompra;
            try
            {
                idCompra = long.Parse(_util.Decrypt(vm.IdCompraEnc));
            }
            catch
            {
                TempData["Error"] = "Identificador inválido.";
                return await Registro();
            }

            var api = _http.CreateClient("Api");
            var payload = new AbonoRequest { Id_Compra = idCompra, Monto = vm.Monto };
            var resp = await api.PostAsJsonAsync("api/abonos", payload);

            if (resp.IsSuccessStatusCode)
                return RedirectToAction("Consulta", "Compras");

            var err = await resp.Content.ReadAsStringAsync();
            TempData["Error"] = !string.IsNullOrWhiteSpace(err) ? err : "Error al registrar el abono.";
            return await Registro();
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