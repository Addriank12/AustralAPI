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
using Microsoft.AspNetCore.Authorization;

namespace AustralAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController(ApplicationDbContext _context) : ControllerBase
    {

        // GET: api/Compras/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Compra>> GetCompra(long id)
        {
            var compra = await _context.Compras
                .Include(f => f.DetalleCompras)
                .Include(f => f.IdProveedorNavigation)
                .SingleAsync(f => f.Id == id);

            if (compra == null)
            {
                return NotFound();
            }

            return compra;
        }

        // GET: api/Compras
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompras(int pagina = 1, int maximoPorPagina = 10, string? filtro = null)
        {
            try
            {
                var compraQuery = _context.Compras.AsQueryable();
                compraQuery = compraQuery.Include(f => f.IdProveedorNavigation);


                if (!string.IsNullOrEmpty(filtro))
                {
                    compraQuery = compraQuery.Where(f => f.IdProveedorNavigation.Nombre.Contains(filtro) || f.IdProveedorNavigation.Nombre.Contains(filtro));
                }

                var totalCompras = await compraQuery.CountAsync();
                var totalPaginas = (int)Math.Ceiling(totalCompras / (double)maximoPorPagina);

                var compras = await compraQuery
                    .Skip((pagina - 1) * maximoPorPagina)
                    .Take(maximoPorPagina)
                    .ToListAsync();

                return Ok(new
                {
                    TotalPages = totalPaginas.ToString(),
                    Data = compras
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/Compras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCompra(long id, Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest();
            }

            _context.Entry(compra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(id))
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
        [Authorize]
        public async Task<ActionResult<Factura>> PostCompra(CompraConDetallesDTO compraDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Crear la factura
                var compra = new Compra
                {
                    Fecha = compraDTO.Fecha,
                    Total = compraDTO.Total,
                    IdProveedor = compraDTO.IdCliente,
                    DetalleCompras= new List<DetalleCompra>()
                };

                // Agregar los detalles de la factura
                foreach (var detalleDTO in compraDTO.Detalles)
                {
                    var detalle = new DetalleCompra
                    {
                        IdProducto = detalleDTO.IdProducto,
                        Cantidad = detalleDTO.Cantidad,
                        Precio = detalleDTO.Precio
                    };
                    compra.DetalleCompras.Add(detalle);

                    var producto = _context.Productos.Single(p => p.Id.Equals(detalle.IdProducto));
                    producto.Stock += detalle.Cantidad;
                    _context.Entry(producto).State = EntityState.Modified;
                }
                _context.Compras.Add(compra);

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCompra", new { id = compra.Id }, compra);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE: api/Compras/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCompra(long id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompraExists(long id)
        {
            return _context.Compras.Any(e => e.Id == id);
        }
    }
}
