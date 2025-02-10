using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AustralAPI.Models;

public partial class Proveedor
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Ingrese el nombre del proveedor")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\\s'-]+$", ErrorMessage = "Ingrese un nombre válido")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Ingrese el teléfono del proveedor")]
    public string Telefono { get; set; } = null!;

    [Required(ErrorMessage = "Ingrese la dirección del proveedor")]
    public string Direccion { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
