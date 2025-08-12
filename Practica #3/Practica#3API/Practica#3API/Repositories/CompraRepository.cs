using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Practica_3API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practica3API.Repositories
{
    public class CompraRepository : ICompraRepository
    {
        private readonly string _cs;
        public CompraRepository(IConfiguration cfg) => _cs = cfg.GetConnectionString("Db");

        public async Task<IEnumerable<Compra>> ListarTodasAsync()
        {
            using var cn = new SqlConnection(_cs);
            return await cn.QueryAsync<Compra>("EXEC dbo.sp_Compras_Listar");
        }

        public async Task<IEnumerable<CompraPend>> ListarPendientesAsync()
        {
            using var cn = new SqlConnection(_cs);
            return await cn.QueryAsync<CompraPend>("EXEC dbo.sp_Compras_ListarPend");
        }

        public async Task<decimal?> ObtenerSaldoAsync(long idCompra)
        {
            using var cn = new SqlConnection(_cs);
            return await cn.ExecuteScalarAsync<decimal?>(
                "EXEC dbo.sp_Compras_Saldo @Id_Compra", new { Id_Compra = idCompra });
        }
    }
}