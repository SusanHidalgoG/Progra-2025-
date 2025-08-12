
using System.Collections.Generic;
using System.Threading.Tasks;
using Practica_3API.Models;

namespace Practica3API.Repositories
{
    public interface ICompraRepository
    {

        Task<IEnumerable<Compra>> ListarTodasAsync();
        Task<IEnumerable<CompraPend>> ListarPendientesAsync();
        Task<decimal?> ObtenerSaldoAsync(long idCompra);

    }
}
