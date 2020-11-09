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
    public class InquilinosController : ControllerBase
    {
        private readonly DataContext contexto;

        public InquilinosController(DataContext context)
        {
            contexto = context;
        }

        // GET: api/Inquilino
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inquilino>>> Get()
        {

            try
            {
                var inquilinos = await contexto.Contratos
                    .Include(contrato => contrato.Inmueble)
                    .ThenInclude(inmueble => inmueble.Duenio)
                    .Include(contrato => contrato.Inquilino)
                    .Where(contrato => contrato.Inmueble.Duenio.Email == User.Identity.Name)
                    .Select(contrato=> contrato.Inquilino)
                    .ToListAsync();

                if (inquilinos == null)
                {
                    return NotFound("No hay ningun inquilino");
                }

                return Ok(inquilinos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // GET: api/Inquilino
        [HttpGet("Inmueble/{id}")]
        public async Task<ActionResult<Inmueble>> GetInmueblesPorInquilino(int id)
        {

            try
            {
                var inquilinos = await contexto.Contratos
                    .Include(contrato => contrato.Inquilino)
                    .Include(contrato => contrato.Inmueble)
                    .ThenInclude(inmueble => inmueble.Duenio)
                    .Where(contrato => contrato.Inquilino.Id == id)
                    .Select(contrato => contrato.Inmueble)
                    .FirstOrDefaultAsync();

                if (inquilinos == null)
                {
                    return NotFound("No hay ningun inquilino");
                }

                return Ok(inquilinos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Inquilino/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inquilino>> Get(int id)
        {
            var inquilino = await contexto.Inquilinos.FindAsync(id);

            if (inquilino == null)
            {
                return NotFound();
            }

            return inquilino;
        }

        // PUT: api/Inquilino/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Inquilino inquilino)
        {
            if (id != inquilino.Id)
            {
                return BadRequest();
            }

            contexto.Entry(inquilino).State = EntityState.Modified;

            try
            {
                await contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InquilinoExists(id))
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

        // POST: api/Inquilino
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Inquilino>> Post(Inquilino inquilino)
        {
            contexto.Inquilinos.Add(inquilino);
            await contexto.SaveChangesAsync();

            return CreatedAtAction("GetInquilino", new { id = inquilino.Id }, inquilino);
        }

        // DELETE: api/Inquilino/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inquilino>> Delete(int id)
        {
            var inquilino = await contexto.Inquilinos.FindAsync(id);
            if (inquilino == null)
            {
                return NotFound();
            }

            contexto.Inquilinos.Remove(inquilino);
            await contexto.SaveChangesAsync();

            return inquilino;
        }

        private bool InquilinoExists(int id)
        {
            return contexto.Inquilinos.Any(e => e.Id == id);
        }
    }
}
