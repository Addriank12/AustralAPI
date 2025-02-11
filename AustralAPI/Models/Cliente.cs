using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AustralAPI.Models;

public partial class Cliente
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Ingrese el nombre del cliente")]
    public string Nombre { get; set; } = null!;
    [Required(ErrorMessage = "Ingrese el teléfono del cliente")]
    [RegularExpression("^\\d+$", ErrorMessage = "Ingrese un teléfono válido")]

    public string Telefono { get; set; } = null!;
    [Required(ErrorMessage = "Ingrese el email del cliente")]
    [EmailAddress(ErrorMessage = "Ingrese una dirección de correo valida")]

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    public string? Direccion { get; set; }

    [JsonIgnore]
    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
