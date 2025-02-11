using AustralAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AustralAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetDashboardData()
        {
            try
            {
                // Total de Ventas (suma de los totales de todas las facturas)
                var totalVentas = _context.Facturas.Sum(f => f.Total);

                // Total de Compras (suma de los totales de todas las compras)
                var totalCompras = _context.Compras.Sum(c => c.Total);

                // Total de Ganancias (ventas - compras)
                var totalGanancias = totalVentas - totalCompras;

                // Total de Clientes (contar todos los clientes)
                var totalClientes = _context.Clientes.Count();

                // Ganancias por Mes (agrupar facturas por mes y sumar los totales)
                var gananciasPorMes = _context.Facturas
                    .GroupBy(f => new { f.Fecha.Year, f.Fecha.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Ganancias = g.Sum(f => f.Total)
                    })
                    .OrderBy(g => g.Year)
                    .ThenBy(g => g.Month)
                    .ToList() // Ejecuta la consulta en la base de datos
                    .Select(g => new
                    {
                        Mes = $"{g.Year}-{g.Month.ToString().PadLeft(2, '0')}",
                        Ganancias = g.Ganancias
                    })
                    .ToList();

                // Datos para el dashboard
                var data = new
                {
                    TotalVentas = totalVentas,
                    TotalCompras = totalCompras,
                    TotalGanancias = totalGanancias,
                    TotalClientes = totalClientes,
                    GananciasPorMes = gananciasPorMes
                };

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}