using System.ComponentModel.DataAnnotations;

namespace Practica_3API.Models
{
    public class AbonoRequest
    {
        [Required]
        public long Id_Compra { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El abono debe ser mayor a cero.")]
        public decimal Monto { get; set; }
    }
}
