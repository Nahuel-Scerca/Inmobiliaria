using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPrueba.Models;

namespace WebApplicationPrueba.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly DataContext contexto;

        public PagosController(DataContext context)
        {
            contexto = context;
        }

        // GET: api/Pagoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPago()
        {
            return await contexto.Pagos.ToListAsync();
        }

        // GET: api/Pagoes
        [HttpGet("Inmueble/{id}")]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetPagosPorInmueble(int id)
        {
            try
            {
                var pagos = await contexto.Pagos
                .Include(pagos => pagos.Contrato)
                .Include(pagos => pagos.Contrato.Inmueble)
                .Include(pagos => pagos.Contrato.Inmueble.Duenio)
                .Include(pagos => pagos.Contrato.Inquilino)
                .Where(pagos => pagos.Contrato.InmuebleId == id && pagos.Contrato.Inmueble.Duenio.Email == User.Identity.Name && pagos.Contrato.FechaDesde <= DateTime.Now && pagos.Contrato.FechaHasta >= DateTime.Now)
                .ToListAsync();
                if (pagos == null)
                {
                    return NotFound("No hay ningun pagos");
                }

                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Pagoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPago(int id, Pago pago)
        {
            if (id != pago.Id)
            {
                return BadRequest();
            }

            contexto.Entry(pago).State = EntityState.Modified;

            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(id))
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

        // POST: api/Pagoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pago>> PostPago(Pago pago)
        {
            contexto.Pagos.Add(pago);
            await contexto.SaveChangesAsync();

            return CreatedAtAction("GetPago", new { id = pago.Id }, pago);
        }

        // DELETE: api/Pagoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pago>> DeletePago(int id)
        {
            var pago = await contexto.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }

            contexto.Pagos.Remove(pago);
            await contexto.SaveChangesAsync();

            return pago;
        }

        private bool PagoExists(int id)
        {
            return contexto.Pagos.Any(e => e.Id == id);
        }
    }
}
