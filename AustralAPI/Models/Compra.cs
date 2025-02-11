using System.ComponentModel.DataAnnotations;

namespace AustralAPI.Models;

public partial class Compra
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Ingrese la fecha de la compra")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "Ingrese el total de la compra")]
    [Range(0, double.MaxValue, ErrorMessage = "Ingrese un total válido")]
    public decimal Total { get; set; }

    public long IdProveedor { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}