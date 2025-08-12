using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Practica3API.Repositories;

namespace Practica_3API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprasController : ControllerBase
    {
        private readonly ICompraRepository _repo;
        public ComprasController(ICompraRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetTodas() =>
            Ok(await _repo.ListarTodasAsync());

        [HttpGet("pendientes")]
        public async Task<IActionResult> GetPendientes() =>
            Ok(await _repo.ListarPendientesAsync());

        [HttpGet("{id:long}/saldo")]
        public async Task<IActionResult> GetSaldo(long id)
        {
            var saldo = await _repo.ObtenerSaldoAsync(id);
            return saldo is null ? NotFound() : Ok(saldo);
        }
    }
}
