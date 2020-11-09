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
    public class ContratosController : ControllerBase
    {
        private readonly DataContext contexto;

        public ContratosController(DataContext context)
        {
            contexto = context;
        }

        // GET: api/Contratoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContrato()
        {
            return await contexto.Contratos.ToListAsync();
        }

        // GET: api/Contratoes
        [HttpGet("Inmueble/{id}")]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContratoPorInmueble(int id)
        {
            try
            {
                var contrato = await contexto.Contratos
                .Include(contrato => contrato.Inmueble)
                .Include(contrato => contrato.Inquilino)
                .Include(contrato => contrato.Inmueble.Duenio)
                .Where(contrato => contrato.InmuebleId == id && contrato.FechaDesde <= DateTime.Now && contrato.FechaHasta >= DateTime.Now && contrato.Inmueble.Duenio.Email == User.Identity.Name)
                .FirstOrDefaultAsync();
                if (contrato == null || contrato.Inmueble.Duenio.Email != User.Identity.Name)
                {
                    return NotFound("No hay ningun contrato");
                }

                return Ok(contrato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Contratoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contrato>> GetContrato(int id)
        {
            var contrato = await contexto.Contratos.FindAsync(id);

            if (contrato == null)
            {
                return NotFound();
            }

            return contrato;
        }

        // PUT: api/Contratoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContrato(int id, Contrato contrato)
        {
            if (id != contrato.Id)
            {
                return BadRequest();
            }

            contexto.Entry(contrato).State = EntityState.Modified;

            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContratoExists(id))
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

        // POST: api/Contratoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Contrato>> PostContrato(Contrato contrato)
        {
            contexto.Contratos.Add(contrato);
            await contexto.SaveChangesAsync();

            return CreatedAtAction("GetContrato", new { id = contrato.Id }, contrato);
        }

        // DELETE: api/Contratoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contrato>> DeleteContrato(int id)
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }

            contexto.Contratos.Remove(contrato);
            await contexto.SaveChangesAsync();

            return contrato;
        }

        private bool ContratoExists(int id)
        {
            return contexto.Contratos.Any(e => e.Id == id);
        }
    }
}
