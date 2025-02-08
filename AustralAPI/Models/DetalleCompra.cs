using System;
using System.Collections.Generic;

namespace AustralAPI.Models;

public partial class DetalleCompra
{
    public long Id { get; set; }

    public long IdCompra { get; set; }

    public long IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal Costo { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
