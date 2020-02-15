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
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapeImportController : ControllerBase
    {
        private readonly CvContext _context;

        /// <summary>
        ///  ctor
        /// </summary>
        public ScrapeImportController(CvContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scrapeRun"></param>
        /// <returns></returns>
        // POST: api/ScrapeRuns
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ScrapeRun ImportScrapeRun(string scrapeRunData)
        {
            var scrapeRun = new ScrapeRun();
            return scrapeRun;
        }


    }
}