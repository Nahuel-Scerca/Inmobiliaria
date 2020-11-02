using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPrueba.Models;

namespace WebApplicationPrueba.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly DataContext _context;

        public ContratosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Contratoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContrato()
        {
            return await _context.Contratos.ToListAsync();
        }

        // GET: api/Contratoes
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContratoPorInmueble(int id)
        {
            var contratos = await _context.Contratos.Where(x => x.InmuebleId == id).ToListAsync();

            if (contratos ==null)
            {
                return NotFound();
            }

            return contratos;
        }

        // GET: api/Contratoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contrato>> GetContrato(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);

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

            _context.Entry(contrato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContrato", new { id = contrato.Id }, contrato);
        }

        // DELETE: api/Contratoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Contrato>> DeleteContrato(int id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }

            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();

            return contrato;
        }

        private bool ContratoExists(int id)
        {
            return _context.Contratos.Any(e => e.Id == id);
        }
    }
}
