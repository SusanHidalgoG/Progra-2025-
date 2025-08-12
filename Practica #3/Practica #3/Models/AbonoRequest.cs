using System.ComponentModel.DataAnnotations;

namespace Practica__3.Models
{
    public class AbonoRequest
    {
        [Required(ErrorMessage = "Seleccione una compra.")]
        public long Id_Compra { get; set; }

        [Required(ErrorMessage = "Ingrese un abono.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El abono debe ser mayor a cero.")]
        public decimal Monto { get; set; }
    }
}
