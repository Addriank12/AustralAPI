using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AustralAPI.Models;

public partial class Producto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Ingrese el nombre del producto")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Ingrese el precio del producto")]
    [Range(0, double.MaxValue, ErrorMessage = "Ingrese un precio válido")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "Ingrese la cantidad en stock")]
    [Range(0, int.MaxValue, ErrorMessage = "Ingrese una cantidad válida")]
    public int Stock { get; set; }

    [JsonIgnore]
    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    [JsonIgnore]
    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();
}
