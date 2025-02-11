using AustralAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace AustralAPI.DataTransfers
{
    public class CompraConDetallesDTO
    {
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public long IdCliente { get; set; }
        public long IdEmpleado { get; set; }

        [Required(ErrorMessage = "Ingrese el tipo de pago")]
        public Pago Pago { get; set; }

        public List<DetalleCompraDTO> Detalles { get; set; } = new List<DetalleCompraDTO>();
    }
}