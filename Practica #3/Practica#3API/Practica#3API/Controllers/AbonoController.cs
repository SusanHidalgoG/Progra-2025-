using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica_3API.Models;
using Practica3API.Repositories;

namespace Practica_3API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonoController : ControllerBase
    {
        private readonly IAbonoRepository _repo;
        public AbonoController(IAbonoRepository repo) => _repo = repo;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AbonoRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _repo.RegistrarAbonoAsync(req.Id_Compra, req.Monto);
                return Ok(new { message = "Abono registrado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}