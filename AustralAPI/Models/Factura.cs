using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AustralAPI.Models;

public partial class Factura
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Ingrese la fecha de la factura")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "Ingrese el total de la factura")]
    public decimal Total { get; set; }

    public long IdCliente { get; set; }

    public long IdEmpleado { get; set; }

    public long IdPago { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Pago IdPagoNavigation { get; set; } = null!;

    public virtual ICollection<ServicioEntrega> ServicioEntregas { get; set; } = new List<ServicioEntrega>();
}
