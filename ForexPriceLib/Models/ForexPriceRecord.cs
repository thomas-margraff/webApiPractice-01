using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

//using CommonExtensions;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace ForexPriceLib.Models
{
    public class ForexPriceRecord : ForexPriceRecordBase
    {
        public enum ShowPriceOptionsEnum
        {
            MidPoint,
            Bid,
            Ask,
            BidAsk,
            BidMidPoint,
            BidAskMidPoint,
            Spread
        }
        public ShowPriceOptionsEnum ShowPriceOptions { get; set; }
        public DateTime PriceDateTimeNoOffset
        {
            get { return this.PriceDateTime.DateTime; }
        }
        public DateTimeOffset PriceDateTime { get; set; }
        public bool IsDst
        {
            get { return PriceDateTime.DateTime.IsDaylightSavingTime(); }
        }
        public string PriceTime
        {
            get
            {
                return this.PriceDateTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            }
        }

        public DateTime PriceDateTimeEstToUtc
        {
            get
            {
                return this.PriceDateTimeConvertToEstNoOffset().ToUniversalTime();
            }
        }

        public DateTime PriceDateTimeEst
        {
            get
            {
                return this.PriceDateTimeConvertToEstNoOffset();
            }
        }
        
        public string PriceTimeEst
        {
            get
            {
                return this.PriceDateTimeConvertToEstNoOffset().ToString("hh:mm tt", CultureInfo.InvariantCulture);
            }
        }
        public string PriceTimeLocal
        {
            get
            {
                return this.PriceDateTime.ToLocalTime().ToString("hh:mm tt", CultureInfo.InvariantCulture);
            }
        }
        
        public bool HasBidAskPrices { get; set; }
        public ForexPriceRecord() { }
        public ForexPriceRecord(ShowPriceOptionsEnum showPriceOptions)
        {
            this.ShowPriceOptions = showPriceOptions;
        }

        public override string ToString()
        {
            // default shows just ohlc
            string ret = string.Format("{0} {1} o={2} h={3} l={4} c={5} r={6} ph={7} pl={8}",
                this.Symbol, this.PriceDateTime, this.Open, this.High, this.Low, this.Close, this.PipsRange, this.PipsHigh, this.PipsLow);

            switch (ShowPriceOptions)
            {
                case ShowPriceOptionsEnum.Bid:
                    ret = string.Format("{0} {1} bo={2} bh={3} bl={4} bc={5} br={6} bph={7} bpl={8}",
                            this.Symbol, this.PriceDateTime, this.BidOpen, this.BidHigh, this.BidLow, this.BidClose, this.PipsHighBid, this.PipsHighBid, this.PipsLowBid);
                    break;

                case ShowPriceOptionsEnum.Ask:
                    ret = string.Format("{0} {1} ao={2} ah={3} al={4} ac={5} ar={6} aph={7} apl={8}",
                            this.Symbol, this.PriceDateTime, this.AskOpen, this.AskHigh, this.AskLow, this.AskClose, this.PipsHighAsk, this.PipsHighAsk, this.PipsLowAsk);
                    break;

                case ShowPriceOptionsEnum.BidAsk:
                    ret = string.Format("{0} {1} bo={2} ao={3} bh={4} ah={5} bl={6} al={7} bc={8} ac={9} phb={10} pha={11} plb={12} pla={13} prb={14} pra={15}",
                            this.Symbol, this.PriceDateTime,
                            this.BidOpen, this.AskOpen,
                            this.BidHigh, this.AskHigh,
                            this.BidLow, this.AskLow,
                            this.BidClose, this.AskClose,
                            this.PipsHighBid, this.PipsHighAsk,
                            this.PipsLowBid, this.PipsLowAsk,
                            this.PipsRangeBid, this.PipsRangeAsk);
                    break;

                case ShowPriceOptionsEnum.Spread:
                    ret = string.Format("{0} {1} ao={2} ah={3} al={4} ac={5} ar={6} aph={7} apl={8}",
                            this.Symbol, this.PriceDateTime, this.AskOpen, this.AskHigh, this.AskLow, this.AskClose, this.PipsHighAsk, this.PipsHighAsk, this.PipsLowAsk);
                    break;
            }

            return ret;
        }

        public static string CsvHeaderOHLC
        {
            get
            {
                return "Time,Symbol,Open,High,Low,Close,PipsHigh,PipsLow,PipsRange";
            }
        }

        public new string ToCsvOHLC()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
               PriceDateTime.ToString("MM/dd/yyyy HH:mm"),
               Symbol,
               Open,
               High,
               Low,
               Close,
               PipsHigh,
               PipsLow,
               PipsRange);
        }

        public static string CsvHeader
        {
            get
            {
                return "Time,SymbolId,Symbol,Open,High,Low,Close,PipsHigh,PipsLow,PipsRange,BidOpen,BidHigh,BidLow,BidClose,PipsHighBid,PipsLowBid,PipsRangeBid,AskOpen,AskHigh,AskLow,AskClose,PipsHighAsk,PipsLowAsk,PipsRangeAsk";
            }
        }
        public new string ToCsv()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23}",
               PriceDateTime.ToString("MM/dd/yyyy HH:mm"),
               SymbolId,
               Symbol,
               Open,
               High,
               Low,
               Close,
               PipsHigh,
               PipsLow,
               PipsRange,
               BidOpen,
               BidHigh,
               BidLow,
               BidClose,
               PipsHighBid,
               PipsLowBid,
               PipsRangeBid,
               AskOpen,
               AskHigh,
               AskLow,
               AskClose,
               PipsHighAsk,
               PipsLowAsk,
               PipsRangeAsk);
        }

        public new string ToCsv1()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
               Symbol,
               PriceDateTime.ToString("MM/dd/yyyy HH:mm"),
               Open,
               High,
               Low,
               Close,
               BidOpen,
               BidHigh,
               BidLow,
               BidClose,
               AskOpen,
               AskHigh,
               AskLow,
               AskClose);
        }

        public DateTime PriceDateTimeConvertToEstNoOffset()
        {
            return PriceDateTimeConvertToEst().DateTime;
        }
        
        public DateTimeOffset PriceDateTimeConvertToEst()
        {
            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return PriceDateTimeConvertTz(tzi);
        }
        
        public DateTimeOffset PriceDateTimeConvertTz(TimeZoneInfo tzi)
        {
            DateTimeOffset dtoGMT = new DateTimeOffset(this.PriceDateTime.DateTime, tzi.BaseUtcOffset).UtcDateTime;
            TimeSpan tsbase = TimeZoneInfo.Local.BaseUtcOffset;
            TimeSpan ts = TimeZoneInfo.Local.GetUtcOffset(dtoGMT.LocalDateTime);
            dtoGMT = this.PriceDateTime.Add(tzi.BaseUtcOffset); // dtoGMT.Add(tzi.BaseUtcOffset);
            return this.PriceDateTime.Add(tzi.BaseUtcOffset);
        }

        public static ForexPriceRecord Create(ForexPriceRecord rec, int symbolid)
        {
            rec.SymbolId = symbolid;
            return rec;
        }

        public string ToCsvPipsRangeEst()
        {
            return string.Format("{0},{1},{2},{3},{4}",
                this.Symbol,
                this.PriceDateTime,
                this.PipsHigh,
                this.PipsLow,
                this.PipsRange);
        }
    }
}


