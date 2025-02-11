using AustralAPI.Data;
using AustralAPI.Models;
using AustralAPI.Security;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AustralAPI.Controllers
{
    [ApiController]

    public class AuthController(ApplicationDbContext _context, Jwt jwt) : Controller
    {

        [HttpPost("register")]
        public IActionResult Register([FromBody] Cliente cliente)
        {
            // Validar el modelo recibido
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, devolver los errores de validación
                return BadRequest(ModelState);
            }

            try
            {
                // Encriptar la contraseña antes de guardarla
                cliente.Password = BCrypt.Net.BCrypt.HashPassword(cliente.Password);

                // Lógica para guardar el cliente en la base de datos
                _context.Clientes.Add(cliente);
                _context.SaveChanges();

                // Devolver una respuesta exitosa
                return Ok(new { Message = "Cliente registrado exitosamente", ClienteId = cliente.Id });
            }
            catch (Exception ex)
            {
                // Manejar errores inesperados
                return StatusCode(500, new { Message = "Ocurrió un error al registrar el cliente", Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Buscar el cliente por su email
            var cliente = _context.Clientes.FirstOrDefault(c => c.Email == request.Email);

            if (cliente == null)
            {
                return NotFound(new { Message = "Cliente no encontrado" });
            }

            // Verificar la contraseña
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, cliente.Password);

            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "Contraseña incorrecta" });
            }

            // Si la contraseña es válida, generar un token de autenticación (opcional)
            var token = jwt.GenerateJwtToken(cliente);

            return Ok(new { Message = "Inicio de sesión exitoso", Token = token, Cliente = cliente.Id});
        }
    }
}
