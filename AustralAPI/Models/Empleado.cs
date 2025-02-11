using System.ComponentModel.DataAnnotations;

namespace AustralAPI.Models;

public partial class Empleado
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Ingrese el nombre del empleado")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Ingrese el email del empleado")]
    public string Cargo { get; set; } = null!;

    [RegularExpression("^\\d+$", ErrorMessage = "Ingrese un teléfono válido")]
    public string Telefono { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}