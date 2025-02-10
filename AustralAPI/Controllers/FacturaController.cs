using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AustralAPI.Data;
using AustralAPI.Models;
using AustralAPI.DataTransfers;
using System.CodeDom.Compiler;

namespace AustralAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController(ApplicationDbContext _context) : ControllerBase
    {        

        // GET: api/Factura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas(int pagina = 1, int maximoPorPagina = 10, string? filtro = null)
        {
            try
            {
                var facturaQuery = _context.Facturas.AsQueryable();
                facturaQuery = facturaQuery.Include(f => f.IdClienteNavigation);
                

                if (!string.IsNullOrEmpty(filtro))
                {   
                    facturaQuery = facturaQuery.Where(f => f.IdClienteNavigation.Nombre.Contains(filtro) || f.IdEmpleadoNavigation.Nombre.Contains(filtro));
                }

                var totalFacturas = await facturaQuery.CountAsync();
                var totalPaginas = (int)Math.Ceiling(totalFacturas / (double)maximoPorPagina);

                var facturas = await facturaQuery
                    .Skip((pagina - 1) * maximoPorPagina)
                    .Take(maximoPorPagina)
                    .ToListAsync();

                return Ok(new
                {
                    TotalPages = totalPaginas.ToString(),
                    Data = facturas
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // GET: api/Factura/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(long id)
        {
            var factura = await _context.Facturas
                .Include(f => f.DetalleFacturas)
                .Include(f => f.IdClienteNavigation)
                .Include(f => f.IdPagoNavigation)
                .FirstOrDefaultAsync(f => f.Id == id);
                

            if (factura == null)
            {
                return NotFound();
            }

            return factura;
        }

        // PUT: api/Factura/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(long id, Factura factura)
        {
            if (id != factura.Id)
            {
                return BadRequest();
            }

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(FacturaConDetallesDTO facturaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Pagos.Add(facturaDTO.Pago);
                await _context.SaveChangesAsync();

                // Crear la factura
                var factura = new Factura
                {
                    Fecha = facturaDTO.Fecha,
                    Total = facturaDTO.Total,
                    IdCliente = facturaDTO.IdCliente,
                    IdEmpleado = facturaDTO.IdEmpleado,
                    IdPago = facturaDTO.Pago.Id,
                    DetalleFacturas = new List<DetalleFactura>()
                };

                // Agregar los detalles de la factura
                foreach (var detalleDTO in facturaDTO.Detalles)
                {
                    var detalle = new DetalleFactura
                    {
                        IdProducto = detalleDTO.IdProducto,
                        Cantidad = detalleDTO.Cantidad,
                        Precio = detalleDTO.Precio,
                        Subtotal = detalleDTO.Subtotal
                    };
                    factura.DetalleFacturas.Add(detalle);
                    var producto = _context.Productos.Single(p => p.Id.Equals(detalle.IdProducto));
                    producto.Stock -= detalle.Cantidad;
                    _context.Entry(producto).State = EntityState.Modified;
                }
                _context.Facturas.Add(factura);
               
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFactura", new { id = factura.Id }, factura);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // DELETE: api/Factura/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(long id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacturaExists(long id)
        {
            return _context.Facturas.Any(e => e.Id == id);
        }
    }
}
