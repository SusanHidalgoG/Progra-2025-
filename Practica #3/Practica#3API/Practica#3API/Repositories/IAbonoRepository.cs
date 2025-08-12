
namespace Practica3API.Repositories
{
    public interface IAbonoRepository
    {
        Task RegistrarAbonoAsync(long idCompra, decimal monto);


    }
}
