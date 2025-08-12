using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Practica3API.Repositories
{
    public class AbonoRepository : IAbonoRepository
    {
        private readonly string _cs;
        public AbonoRepository(IConfiguration cfg) => _cs = cfg.GetConnectionString("Db");

        public async Task RegistrarAbonoAsync(long idCompra, decimal monto)
        {
            using var cn = new SqlConnection(_cs);
            await cn.ExecuteAsync("EXEC dbo.sp_Abonos_Registrar @Id_Compra, @Monto",
                                  new { Id_Compra = idCompra, Monto = monto });
        }
    }
}