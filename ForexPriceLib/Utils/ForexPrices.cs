using ForexPriceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForexPriceLib.Utils
{
    public enum Periodicity
    {
        OneMinute = 1,
        TwoMinute = 2,
        ThreeMinute = 3,
        FourMinute = 4,
        FiveMinute = 5,
        TenMinute = 10,
        FifteenMinute = 15,
        SixtyMinute = 60
    }

    /// <summary>
    /// container for prices that span a timeframe
    /// prices can include multiple symbols
    /// </summary>
    public class ForexPrices
    {
        public ForexPrices()
        {
            _downloadExceptions = new List<string>();
            _priceRecords = new List<ForexPriceRecord>();
        }

        private List<string> _downloadExceptions;
        private List<string> _symbols;
        private List<ForexTimePeriodStats> _symbolTimeStats;
        private List<ForexPriceRecord> _priceRecords;
        public DayOfWeek PricesStartDayOfWeek
        {
            get { return this.PriceDateTimeStart.Date.DayOfWeek; }
        }
        public DayOfWeek PricesStopDayOfWeek
        {
            get { return this.PriceDateTimeStop.Date.DayOfWeek; }
        }
        public DateTimeOffset PriceDateTimeStart
        {
            get
            {
                if (this.PriceRecords != null && this.PriceRecords.Count > 0)
                    return (from p in PriceRecords select p.PriceDateTime).Min();
                else
                    return DateTimeOffset.MinValue;
            }
        }
        public DateTimeOffset PriceDateTimeStop
        {
            get
            {
                if (PriceRecords != null || PriceRecords.Count > 0)
                    return (from p in PriceRecords select p.PriceDateTime).Max();
                else
                    return DateTimeOffset.MaxValue;
            }
        }
        public TimeSpan PriceTimeStart
        {
            get
            {
                if (PriceRecords != null || PriceRecords.Count > 0)
                    return (from p in PriceRecords select p.PriceDateTime.TimeOfDay).Min();
                else
                    return TimeSpan.MinValue;
            }
        }
        public TimeSpan PriceTimeStop
        {
            get
            {
                if (PriceRecords != null || PriceRecords.Count > 0)
                    return (from p in PriceRecords select p.PriceDateTime.TimeOfDay).Max();
                else
                    return TimeSpan.MaxValue;
            }
        }
        public List<ForexPriceRecord> PriceRecords
        {
            get { return _priceRecords; }
            set { _priceRecords = value; }
        }

        public List<ForexTimePeriodStats> SymbolTimeStats
        {
            get
            {
                if (_symbolTimeStats == null)
                {
                    _symbolTimeStats = new List<ForexTimePeriodStats>();
                    foreach (string symbol in this.Symbols)
                    {
                        List<ForexPriceRecord> priceRecs = (from p in this.PriceRecords
                                                            where p.Symbol == symbol
                                                            select p).ToList();

                        _symbolTimeStats.Add(priceRecs.OHLCStats(symbol));
                    }
                }
                return _symbolTimeStats;
            }
        }
        public List<string> Symbols
        {
            get
            {
                if (_symbols == null)
                    _symbols = (from p in this.PriceRecords orderby p.Symbol select p.Symbol).Distinct().ToList();
                return _symbols;
            }
        }

        public List<PricesFileAudit> Audit
        {
            get
            {
                List<PricesFileAudit> auditRecs = new List<PricesFileAudit>();

                foreach (string symbol in this.Symbols)
                {
                    var recs = (from p in this.PriceRecords
                                orderby p.PriceDateTime
                                where p.Symbol == symbol
                                select p).ToList();

                    var dates = (from r in recs
                                 select r.PriceDateTime.Date).Distinct().ToList();

                    foreach (var date in dates)
                    {
                        PricesFileAudit audit = new PricesFileAudit()
                        {
                            Symbol = symbol,
                            PriceDate = date,
                            PriceDateBegin = recs.First().PriceDateTime.DateTime,
                            PriceDateEnd = recs.Last().PriceDateTime.DateTime,
                            PriceRecordCount = recs.Count()
                        };
                        auditRecs.Add(audit);
                    }
                }
                return auditRecs;
            }
        }

        public List<string> DownloadExceptions
        {
            get { return _downloadExceptions; }
            set { _downloadExceptions = value; }
        }
    }

    public static class ForexPriceRecordExt
    {
        // not yest implimented
        public static List<ForexPriceRecord> Compress(this List<ForexPriceRecord> recs, string symbol, Periodicity period)
        {
            if (period == Periodicity.OneMinute)
                return recs;

            List<ForexPriceRecord> cp = recs.Where(r => r.Symbol.Equals(symbol)).OrderBy(p => p.PriceDateTime).ToList();
            if (cp.Count < 2)
                return cp;

            List<ForexPriceRecord> newRecs = new List<ForexPriceRecord>();
            List<ForexPriceRecord> tempRecs = new List<ForexPriceRecord>();
            ForexPriceRecord tempRec = new ForexPriceRecord();

            ForexPriceRecord firstRec = cp.First();
            ForexPriceRecord lastRec = cp.Last();

            int minutes = (int)period;

            double OpenPrice = firstRec.Open;
            double BidOpenPrice = firstRec.BidOpen;
            double AskOpenPrice = firstRec.AskOpen;
            double mult = firstRec.PipMultiplier;


            #region csv recs in 1 minute tf
            /*
            AUDCAD	02/04/2009 12:00:00 AM +00:00	0	0	0	0	0.79936	0.79953	0.79932	0.79953	0.80073	0.80085	0.8005	0.80081
            AUDCAD	02/04/2009 12:01:00 AM +00:00	0	0	0	0	0.79953	0.79973	0.79945	0.79973	0.80081	0.80092	0.80081	0.80092
            AUDCAD	02/04/2009 12:02:00 AM +00:00	0	0	0	0	0.79973	0.79993	0.79925	0.79957	0.80092	0.80144	0.80064	0.80076
            AUDCAD	02/04/2009 12:03:00 AM +00:00	0	0	0	0	0.79957	0.79957	0.79905	0.79909	0.80076	0.80076	0.80016	0.80022
            AUDCAD	02/04/2009 12:04:00 AM +00:00	0	0	0	0	0.79909	0.79959	0.79909	0.79943	0.80022	0.8008	0.80022	0.80059
            AUDCAD	02/04/2009 12:05:00 AM +00:00	0	0	0	0	0.79943	0.79953	0.79931	0.79948	0.80059	0.80073	0.80047	0.80068
            AUDCAD	02/04/2009 12:06:00 AM +00:00	0	0	0	0	0.79948	0.79954	0.79936	0.79936	0.80068	0.80075	0.80056	0.80056
            AUDCAD	02/04/2009 12:07:00 AM +00:00	0	0	0	0	0.79936	0.79961	0.79903	0.79959	0.80056	0.80091	0.80044	0.8008
            AUDCAD	02/04/2009 12:08:00 AM +00:00	0	0	0	0	0.79959	0.79959	0.79941	0.79948	0.8008	0.8008	0.80061	0.80068
            AUDCAD	02/04/2009 12:09:00 AM +00:00	0	0	0	0	0.79948	0.80004	0.79936	0.80004	0.80068	0.80123	0.80056	0.80123
            AUDCAD	02/04/2009 12:10:00 AM +00:00	0	0	0	0	0.80004	0.80039	0.8	0.80039	0.80123	0.80149	0.80117	0.80149
            AUDCAD	02/04/2009 12:11:00 AM +00:00	0	0	0	0	0.80039	0.80045	0.79989	0.80035	0.80149	0.80155	0.80136	0.80148
            AUDCAD	02/04/2009 12:12:00 AM +00:00	0	0	0	0	0.80035	0.80043	0.80029	0.80029	0.80148	0.80154	0.80142	0.80142
            AUDCAD	02/04/2009 12:13:00 AM +00:00	0	0	0	0	0.80029	0.80035	0.80018	0.8002	0.80142	0.80142	0.80136	0.80136
            AUDCAD	02/04/2009 12:14:00 AM +00:00	0	0	0	0	0.8002	0.80037	0.79963	0.79989	0.80136	0.80156	0.80107	0.80107
            AUDCAD	02/04/2009 12:15:00 AM +00:00	0	0	0	0	0.79989	0.80012	0.79965	0.79972	0.80107	0.80129	0.80085	0.80091
            AUDCAD	02/04/2009 12:16:00 AM +00:00	0	0	0	0	0.79972	0.80041	0.79972	0.79984	0.80091	0.80176	0.80091	0.80119
            AUDCAD	02/04/2009 12:17:00 AM +00:00	0	0	0	0	0.79984	0.80049	0.79984	0.80034	0.80119	0.80174	0.80119	0.80152
            AUDCAD	02/04/2009 12:18:00 AM +00:00	0	0	0	0	0.80034	0.80093	0.79982	0.80062	0.80152	0.80224	0.80151	0.80193
            AUDCAD	02/04/2009 12:19:00 AM +00:00	0	0	0	0	0.80062	0.80074	0.80019	0.80019	0.80193	0.80198	0.80147	0.80147
            */

            #endregion

            DateTimeOffset lastDto = lastRec.PriceDateTime;
            DateTimeOffset currDto = firstRec.PriceDateTime;
            bool hasBidAskPrices = firstRec.HasBidAskPrices;
            ForexPriceRecord.ShowPriceOptionsEnum showPriceOpts = firstRec.ShowPriceOptions;
            int symbolId = firstRec.SymbolId;

            while (currDto <= lastDto)
            {
                DateTimeOffset endPeriodDto = currDto.AddMinutes(minutes);

                if (endPeriodDto > lastDto)
                    endPeriodDto = lastDto.AddMinutes(10);

                ForexPrices pr = new ForexPrices()
                {
                    PriceRecords = (from r in recs
                                    where r.PriceDateTime >= currDto & r.PriceDateTime < endPeriodDto
                                    select r).ToList()
                };

                var ts = pr.SymbolTimeStats.FirstOrDefault() as ForexPriceRecordBase;
                tempRecs.Add(new ForexPriceRecord()
                    {
                        BidOpen = ts.BidOpen,
                        BidHigh = ts.BidHigh,
                        BidLow = ts.BidLow,
                        BidClose = ts.BidClose,
                        AskOpen = ts.AskOpen,
                        AskHigh = ts.AskHigh,
                        AskLow = ts.AskLow,
                        AskClose = ts.AskClose,
                        Open = ts.Open,
                        High = ts.High,
                        Low = ts.Low,
                        Close = ts.Close,
                        HasBidAskPrices = hasBidAskPrices,
                        PriceDateTime = currDto,
                        ShowPriceOptions = showPriceOpts,
                        Symbol = symbol,
                        SymbolId = symbolId
                    });

                currDto = endPeriodDto;

                if (currDto >= lastDto)
                    break;
            }

            return tempRecs;
        }

        #region ohlc stats
        /// <summary>
        /// returns ohlc stats for this symbol
        /// </summary>
        /// <param name="records"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static ForexTimePeriodStats OHLCStats(this List<ForexPriceRecord> records, string symbol)
        {
            return (from r in records where r.Symbol == symbol select r).ToList().OHLCStats();
        }
        /// <summary>
        /// returns ohlc stats for the price series - all symbols
        /// </summary>
        /// <param name="records"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static ForexTimePeriodStats OHLCStats(this List<ForexPriceRecord> records)
        {
            string symbol;
            if (records.Count == 0)
                return new ForexTimePeriodStats();

            symbol = records[0].Symbol;

            ForexTimePeriodStats stats =
                new ForexTimePeriodStats()
                {
                    SymbolId = records[0].SymbolId,
                    Symbol = symbol,
                    DateTimeBegin = records[0].PriceDateTime,
                    DateTimeEnd = records[records.Count - 1].PriceDateTime,
                    Open = records.Open(),
                    High = records.High(),
                    Low = records.Low(),
                    Close = records.Close(),
                    BidOpen = records.BidOpen(),
                    BidHigh = records.BidHigh(),
                    BidLow = records.BidLow(),
                    BidClose = records.BidClose(),
                    AskOpen = records.AskOpen(),
                    AskHigh = records.AskHigh(),
                    AskLow = records.AskLow(),
                    AskClose = records.AskClose()
                };

            return stats;
        }

        #endregion

        #region open/high/low close
        /// <summary>
        ///  returns the Open price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double Open(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.Open).First();
        }
        /// <summary>
        ///  returns the BidOpen price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double BidOpen(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.BidOpen).First();
        }
        /// <summary>
        ///  returns the AskOpen price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double AskOpen(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.AskOpen).First();
        }

        /// <summary>
        ///  returns the High price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double High(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.High).Max();
        }
        /// <summary>
        ///  returns the BidHigh price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double BidHigh(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.BidHigh).Max();
        }
        /// <summary>
        ///  returns the AskHigh price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double AskHigh(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.AskHigh).Max();
        }

        /// <summary>
        ///  returns the Low price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double Low(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.Low).Min();
        }
        /// <summary>
        ///  returns the BidLow price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double BidLow(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.BidLow).Min();
        }
        /// <summary>
        ///  returns the AskLow price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double AskLow(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.AskLow).Min();
        }

        /// <summary>
        ///  returns the Close price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double Close(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.Close).Last();
        }
        /// <summary>
        ///  returns the BidClose price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double BidClose(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.BidClose).Last();
        }
        /// <summary>
        ///  returns the AskClose price record in the sequence
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static double AskClose(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.AskClose).Last();
        }
        #endregion

        /// <summary>
        /// convert all recs to pips from open
        /// </summary>
        /// <param name="recs"></param>
        /// <returns></returns>
        public static List<ForexPriceRecord> ToPips(this List<ForexPriceRecord> recs)
        {
            ForexPrices fxp = new ForexPrices()
            {
                PriceRecords = recs
            };

            List<ForexPriceRecord> pipsRecords = new List<ForexPriceRecord>();
            List<string> symbols = fxp.Symbols;
            foreach (var symbol in symbols)
            {
                pipsRecords.AddRange(recs.ToPips(symbol));
            }
            return pipsRecords;
        }

        public static string ToPipsHeader()
        {
            return "";
        }
        /// <summary>
        /// convert all recs to pips from open for a symbol
        /// </summary>
        /// <param name="recs"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static List<ForexPriceRecord> ToPips(this List<ForexPriceRecord> recs, string symbol)
        {
            List<ForexPriceRecord> symbolRecs = recs.Where(r => r.Symbol.Equals(symbol)).OrderBy(p => p.PriceDateTime).ToList();
            List<ForexPriceRecord> toPips = new List<ForexPriceRecord>();

            double OpenPrice = recs.First().Open;
            double BidOpenPrice = recs.First().BidOpen;
            double AskOpenPrice = recs.First().AskOpen;
            double mult = recs.First().PipMultiplier;

            symbolRecs.ForEach(rec =>
            {
                toPips.Add(new ForexPriceRecord()
                {
                    PriceDateTime = rec.PriceDateTime,
                    HasBidAskPrices = rec.HasBidAskPrices,
                    ShowPriceOptions = rec.ShowPriceOptions,
                    Symbol = rec.Symbol,
                    SymbolId = rec.SymbolId,

                    Open = (int)Math.Round((rec.Open - OpenPrice) * mult),
                    High = (int)Math.Round((rec.High - OpenPrice) * mult),
                    Low = (int)Math.Round((rec.Low - OpenPrice) * mult),
                    Close = (int)Math.Round((rec.Close - OpenPrice) * mult),

                    BidOpen = (int)Math.Round((rec.BidOpen - BidOpenPrice) * mult),
                    BidHigh = (int)Math.Round((rec.BidHigh - BidOpenPrice) * mult),
                    BidLow = (int)Math.Round((rec.BidLow - BidOpenPrice) * mult),
                    BidClose = (int)Math.Round((rec.BidClose - BidOpenPrice) * mult),

                    AskOpen = (int)Math.Round((rec.AskOpen - AskOpenPrice) * mult),
                    AskHigh = (int)Math.Round((rec.AskHigh - AskOpenPrice) * mult),
                    AskLow = (int)Math.Round((rec.AskLow - AskOpenPrice) * mult),
                    AskClose = (int)Math.Round((rec.AskClose - AskOpenPrice) * mult),

                });
            });

            return toPips;

        }
    }

}

