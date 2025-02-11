using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AustralAPI.Models;

public partial class DetalleFactura
{
    public long Id { get; set; }

    public long IdFactura { get; set; }

    public long IdProducto { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Ingrese un precio válido")]
    [Required(ErrorMessage = "Ingrese el precio")]
    public decimal Precio { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Ingrese una cantidad válida")]
    [Required(ErrorMessage = "Ingrese la cantidad")]
    public int Cantidad { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Ingrese un precio válido")]
    [Required(ErrorMessage = "Ingrese el precio")]
    public decimal Subtotal { get; set; }

    [JsonIgnore]
    public virtual Factura IdFacturaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}