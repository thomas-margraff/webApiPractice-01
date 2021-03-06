﻿using DAL_SqlServer.Dto;
using DAL_SqlServer.Models;
using DAL_SqlServer.SearchModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_SqlServer.Repository
{
    public class IndicatorDataRepository<TDbContext> : IIndicatorDataRepository where TDbContext : DbContext
    {
        protected TDbContext dbContext;

        public IndicatorDataRepository(TDbContext context)
        {
            dbContext = context;
        }

        public async Task<List<IndicatorData>> GetByCurrency(string currency)
        {
            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.Currency == currency)
                .OrderByDescending(r => r.ReleaseDateTime)
                .Take(300)
                .ToListAsync();
        }

        public async Task<List<IndicatorData>> GetIndicatorsForDate(DateTime dt)
        {
            DateTime dtNext = dt.AddDays(1);
            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.ReleaseDateTime >= dt && r.ReleaseDateTime < dtNext)
                .OrderBy(r => r.ReleaseDateTime)
                .ToListAsync();
        }

        public IndicatorData Create(IndicatorData data)
        {
            throw new NotImplementedException();
        }

        public void Create(List<IndicatorData> data)
        {

        }

        public List<IndicatorData> BulkUpdate(List<IndicatorData> recs)
        {
             List<IndicatorData> updRecs = new List<IndicatorData>();

            foreach (var rec in recs)
            {
                var exist = this.dbContext.Set<IndicatorData>().Where(r => r.EventId == rec.EventId).FirstOrDefault();
                if (exist == null)
                {
                    rec.CreateDate = DateTime.Now;
                    this.dbContext.Set<IndicatorData>().Add(rec);
                }
                else
                {
                    exist.ModifyDate = DateTime.Now;
                    exist.Actual = rec.Actual;
                    exist.Forecast = rec.Forecast;
                    exist.Indicator = rec.Indicator;
                    exist.Previous = rec.Previous;
                    exist.ReleaseDate = rec.ReleaseDate;
                    exist.ReleaseDateTime = rec.ReleaseDateTime;
                    exist.ReleaseTime = rec.ReleaseTime;

                    this.dbContext.Entry(exist).State = EntityState.Modified;
                    updRecs.Add(exist);
                }
            }

            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var ccyRecs = this.BulkUpdateCcyNames();

            // scrape history
            IndicatorDataScrapeHistory hist = new IndicatorDataScrapeHistory();
            hist.ScrapeDate = DateTime.Now;
            hist.RecordCount = recs.Count();
            this.dbContext.Set<IndicatorDataScrapeHistory>().Add(hist);

            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return updRecs;
        }

        public List<Symbols> BulkUpdateSymbols(List<string> symbols)
        {
            var syms = new List<Symbols>();
            foreach (var symbol in symbols)
            {
                var exist = this.dbContext.Set<Symbols>().Where(r => r.SymbolCode == symbol).FirstOrDefault();
                if (exist == null)
                {
                    var sym = new Symbols { 
                        IsActive = true, 
                        SymbolCode = symbol, 
                        DateCreate = DateTime.Now 
                    };
                    this.dbContext.Set<Symbols>().Add(sym);
                    syms.Add(sym);
                }
            }
            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return syms;
        }

        public List<IndicatorDataCcyName> BulkUpdateCcyNames()
        {
            // ccy indicator table
            var ccyRecs = (from r in this.dbContext.Set<IndicatorData>()
                           select new ReleaseDto()
                           {
                               Currency = r.Currency,
                               Indicator = r.Indicator
                           }).Distinct().OrderBy(r => r.Currency).ThenBy(d => d.Indicator).ToList();

            List<IndicatorDataCcyName> updateRecs = new List<IndicatorDataCcyName>();

            foreach (var rec in ccyRecs)
            {
                var exist = this.dbContext.Set<IndicatorDataCcyName>()
                            .Where(r => r.Currency == rec.Currency && r.Indicator == rec.Indicator)
                            .FirstOrDefault();

                if (exist == null)
                {
                    var newRec = new IndicatorDataCcyName
                    {
                        Id = 0,
                        CreateDate = DateTime.Now,
                        Currency = rec.Currency,
                        Indicator = rec.Indicator,
                        IsActive = true
                    };
                    this.dbContext.Set<IndicatorDataCcyName>().Add(newRec);
                }
                else
                {
                    exist.ModifyDate = DateTime.Now;
                    exist.Currency = rec.Currency;
                    exist.Indicator = rec.Indicator;

                    this.dbContext.Entry(exist).State = EntityState.Modified;
                    updateRecs.Add(exist);
                }
            }
            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this.dbContext.Set<IndicatorDataCcyName>().ToList();
        }

        public async Task<List<vwCountryIndicator>> GetCurrencyIndicators()
        {
            return await this.dbContext.Set<vwCountryIndicator>()
                .OrderBy(r => r.Currency).ThenBy(r => r.Indicator)
                .ToListAsync();
        }

        public async Task<List<vwCountryIndicator>> GetCurrencyIndicatorsByCcy(string currency)
        {
            return await this.dbContext.Set<vwCountryIndicator>()
                .Where(r => r.Currency == currency)
                .OrderBy(r => r.Currency).ThenBy(r => r.Indicator)
                .ToListAsync();
        }

        public async Task<List<IndicatorData>> ThisWeek()
        {
            var dtToday = DateTime.Now;
            var dayOfWeek = Convert.ToInt16(dtToday.DayOfWeek);
            var sun = dtToday.AddDays(dayOfWeek * -1);
            var sat = sun.AddDays(6);

            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.ReleaseDateTime >= sun && r.ReleaseDateTime <= sat)
                .OrderBy(r => r.ReleaseDateTime)
                .ToListAsync();
        }

        public async Task<List<IndicatorData>> NextWeek()
        {
            var dt = DateTime.Now;
            var dtSunday = new GregorianCalendar().AddDays(dt, -((int)dt.DayOfWeek) + 7);
            var dtFriday = dtSunday.AddDays(6);

            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.ReleaseDateTime >= dtSunday && r.ReleaseDateTime <= dtFriday)
                .OrderBy(r => r.ReleaseDateTime)
                .ToListAsync();
        }

        public async Task<List<IndicatorData>> LastWeek()
        {
            var dt = DateTime.Now;
            var dtSunday = new System.Globalization.GregorianCalendar().AddDays(dt, -((int)dt.DayOfWeek) - 7);
            var dtFriday = dtSunday.AddDays(6);

            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.ReleaseDateTime >= dtSunday && r.ReleaseDateTime <= dtFriday)
                .OrderBy(r => r.ReleaseDateTime)
                .ToListAsync();
        }

        public async Task<List<IndicatorData>> IndicatorsToday()
        {
            var dtToday = DateTime.Now;

            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.ReleaseDateTime >= dtToday.Date &&
                       r.ReleaseDateTime <= dtToday.Date.AddDays(1).AddMinutes(-1))
                .OrderBy(r => r.ReleaseDateTime)
                .ToListAsync();
        }
        public async Task<List<string>> CountriesGetAll()
        {
            // var ccy = IndicatorData.Select(r => r.Currency).Distinct().OrderBy(r => r).Dump();
            return await this.dbContext.Set<IndicatorData>()
                .Select(r => r.Currency)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

        }
        public List<ReleaseDto> IndicatorsGroupByCcyIndicator(string currency)
        {
            var dtos = (from r in this.dbContext.Set<IndicatorData>()
                        where r.Currency == currency
                        select new ReleaseDto()
                        {
                            Id = 0,
                            Currency = r.Currency,
                            Indicator = r.Indicator
                        }).Distinct().OrderBy(d => d.Indicator).ToList();

            foreach (var dto in dtos)
            {
                dto.IndicatorDataDto = (from r in this.dbContext.Set<IndicatorData>()
                                        where r.Currency == dto.Currency && r.Indicator == dto.Indicator
                                        select new IndicatorDataDto()
                                        {
                                            Actual = r.Actual,
                                            EventId = r.EventId,
                                            Forecast = r.Forecast,
                                            IndicatorId = r.Id,
                                            Previous = r.Previous,
                                            ReleaseDate = r.ReleaseDate,
                                            ReleaseDateTime = r.ReleaseDateTime,
                                            ReleaseTime = r.ReleaseTime
                                        }).OrderByDescending(idd => idd.ReleaseDateTime).ToList();
            }

            return dtos;
        }
        public List<ReleaseDto> GetIndicatorsGroupByCcyIndicatorName(string currency, string indicatorName)
        {
            var dtos = (from r in this.dbContext.Set<IndicatorData>()
                        where r.Currency == currency && r.Indicator == indicatorName
                        select new ReleaseDto()
                        {
                            Id = 0,
                            Currency = r.Currency,
                            Indicator = r.Indicator
                        }).Distinct().OrderBy(d => d.Indicator).ToList();

            foreach (var dto in dtos)
            {
                dto.IndicatorDataDto = (from r in this.dbContext.Set<IndicatorData>()
                                        where r.Currency == dto.Currency && r.Indicator == dto.Indicator
                                        select new IndicatorDataDto()
                                        {
                                            Actual = r.Actual,
                                            EventId = r.EventId,
                                            Forecast = r.Forecast,
                                            IndicatorId = r.Id,
                                            Previous = r.Previous,
                                            ReleaseDate = r.ReleaseDate,
                                            ReleaseDateTime = r.ReleaseDateTime,
                                            ReleaseTime = r.ReleaseTime
                                        }).OrderByDescending(idd => idd.ReleaseDateTime).ToList();
            }

            return dtos;
        }
        public async Task<List<IndicatorData>> GetIndicatorHistory(IndicatorDataSearchModel search)
        {
            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.Currency == search.Currency && r.Indicator == search.Indicator)
                .OrderByDescending(r => r.ReleaseDateTime)
                .ToListAsync();
        }
        public async Task<List<IndicatorData>> GetIndicatorsForCcyAndName(string ccy, string indicatorName)
        {
            return await this.dbContext.Set<IndicatorData>()
                .Where(r => r.Currency == ccy && r.Indicator == indicatorName)
                .OrderByDescending(r => r.ReleaseDateTime)
                .ToListAsync();
        }

        public async Task<Configuration> GetConfig(string name)
        {
            return await this.dbContext.Set<Configuration>()
                .Where(r => r.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
