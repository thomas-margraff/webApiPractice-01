using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoronaVirusDAL;
using CoronaVirusDAL.Entities;

namespace CoronaVirusApi.Controllers
{
    /// <summary>
    /// class
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapeRunsController : ControllerBase
    {
        private readonly CvContext _context;

        /// <summary>
        ///  ctor
        /// </summary>
        public ScrapeRunsController(CvContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/ScrapeRuns 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScrapeRun>>> GetScrapeRuns()
        {
            var sr = await _context
                .ScrapeRuns
                .OrderByDescending(r => r.ScrapeDate)
                .ToListAsync();

            return sr;
        }

        /// <summary>
        /// GET: api/ScrapeRuns/5
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ScrapeRun>> GetScrapeRun(int id)
        {
            var scrapeRun = await _context.ScrapeRuns.FindAsync(id);

            if (scrapeRun == null)
            {
                return NotFound();
            }

            return scrapeRun;
        }

        /// <summary>
        /// <param name="id"></param>
        /// <param name="scrapeRun"></param>
        /// <returns></returns>
        /// PUT: api/ScrapeRuns/5
        /// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        /// more details see https://aka.ms/RazorPagesCRUD.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScrapeRun(int id, ScrapeRun scrapeRun)
        {
            if (id != scrapeRun.Id)
            {
                return BadRequest();
            }

            _context.Entry(scrapeRun).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScrapeRunExists(id))
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

        // POST: api/ScrapeRuns
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ScrapeRun>> PostScrapeRun(ScrapeRun scrapeRun)
        {
            _context.ScrapeRuns.Add(scrapeRun);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScrapeRun", new { id = scrapeRun.Id }, scrapeRun);
        }

        // DELETE: api/ScrapeRuns/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ScrapeRun>> DeleteScrapeRun(int id)
        {
            var scrapeRun = await _context.ScrapeRuns.FindAsync(id);
            if (scrapeRun == null)
            {
                return NotFound();
            }

            _context.ScrapeRuns.Remove(scrapeRun);
            await _context.SaveChangesAsync();

            return scrapeRun;
        }

        /// <summary>
        /// ScrapeRunExists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ScrapeRunExists(int id)
        {
            return _context.ScrapeRuns.Any(e => e.Id == id);
        }
    }
}
