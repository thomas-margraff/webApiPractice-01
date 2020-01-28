using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapiDatabaseFirst.Models;

namespace webapiDatabaseFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NtpController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public NtpController(CoreDbContext context)
        {
            _context = context;
        }

        // GET: api/Ntp
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IndicatorDataCcyName>>> GetIndicatorDataCcyName()
        {
            return await _context.IndicatorDataCcyName.ToListAsync();
        }

        // GET: api/Ntp/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IndicatorDataCcyName>> GetIndicatorDataCcyName(int id)
        {
            var indicatorDataCcyName = await _context.IndicatorDataCcyName.FindAsync(id);

            if (indicatorDataCcyName == null)
            {
                return NotFound();
            }

            return indicatorDataCcyName;
        }

        // PUT: api/Ntp/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndicatorDataCcyName(int id, IndicatorDataCcyName indicatorDataCcyName)
        {
            if (id != indicatorDataCcyName.Id)
            {
                return BadRequest();
            }

            _context.Entry(indicatorDataCcyName).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IndicatorDataCcyNameExists(id))
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

        // POST: api/Ntp
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<IndicatorDataCcyName>> PostIndicatorDataCcyName(IndicatorDataCcyName indicatorDataCcyName)
        {
            _context.IndicatorDataCcyName.Add(indicatorDataCcyName);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIndicatorDataCcyName", new { id = indicatorDataCcyName.Id }, indicatorDataCcyName);
        }

        // DELETE: api/Ntp/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IndicatorDataCcyName>> DeleteIndicatorDataCcyName(int id)
        {
            var indicatorDataCcyName = await _context.IndicatorDataCcyName.FindAsync(id);
            if (indicatorDataCcyName == null)
            {
                return NotFound();
            }

            _context.IndicatorDataCcyName.Remove(indicatorDataCcyName);
            await _context.SaveChangesAsync();

            return indicatorDataCcyName;
        }

        private bool IndicatorDataCcyNameExists(int id)
        {
            return _context.IndicatorDataCcyName.Any(e => e.Id == id);
        }
    }
}
