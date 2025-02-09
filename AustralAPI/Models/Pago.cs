using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AustralAPI.Models;

public partial class Pago
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Ingrese la fecha del pago")]
    public string Metodo { get; set; } = null!;

    [Required(ErrorMessage = "Ingrese el monto del pago")]
    [Range(0, double.MaxValue, ErrorMessage = "Ingrese un monto válido")]
    public decimal Monto { get; set; }

    [JsonIgnore]
    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
