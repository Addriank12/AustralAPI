using System.ComponentModel.DataAnnotations;

namespace AustralAPI.Models;

public partial class ServicioEntrega
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Ingrese la dirección de entrega")]
    public string Empresa { get; set; } = null!;

    [Required(ErrorMessage = "Ingrese la dirección de entrega")]
    public string Estado { get; set; } = null!;

    public long IdFactura { get; set; }

    public virtual Factura IdFacturaNavigation { get; set; } = null!;
}