using System.Text.Json.Serialization;

namespace AustralAPI.Models;

public partial class DetalleCompra
{
    public long Id { get; set; }

    public long IdCompra { get; set; }

    public long IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    [JsonIgnore]
    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}