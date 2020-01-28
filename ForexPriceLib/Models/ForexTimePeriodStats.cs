using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForexPriceLib.Models
{
    public partial class ForexTimePeriodStats : ForexPriceRecordBase
    {
        // used to calulate pip value
        public DateTimeOffset DateTimeBeginAnchor { get; set; }
        public DateTimeOffset DateTimeEndAnchor { get; set; }
        
        public DateTimeOffset DateTimeBegin { get; set; }
        public DateTimeOffset DateTimeEnd { get; set; }

        // ctor
        public ForexTimePeriodStats() { }

        public static string ToTimeStatCsvHeader()
        {
            return "DateTimeBegin,DateTimeEnd,Symbol,Open,High,Low,Close,PipsHigh,PipsLow,PipsRange";
        }

        public string ToTimeStatCsv()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                                 DateTimeBegin.ToString("MM/dd/yyyy HH:mm"),
                                 DateTimeEnd.ToString("MM/dd/yyyy HH:mm"),
                                 Symbol, Open, High, Low, Close,
                                 PipsHigh, PipsLow, PipsRange);

        }

        public string ToCsvAll()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24}",
                DateTimeBegin.ToString("MM/dd/yyyy HH:mm"),
                DateTimeEnd.ToString("MM/dd/yyyy HH:mm"), 
                SymbolId, Symbol, Open, High, Low, Close, BidOpen, BidHigh, BidLow, BidClose, AskOpen, AskHigh, AskLow, AskClose,
                PipsHigh, PipsLow, PipsRange, PipsHighBid, PipsLowBid, PipsRangeBid, PipsHighAsk, PipsLowAsk, PipsRangeAsk);

        }

        public static string ToCsvAllHeaderRecord()
        {
            return "BeginDT,EndDT,SymbolId,Symbol,Open,High,Low,Close,BidOpen,BidHigh,BidLow,BidClose,AskOpen,AskHigh,AskLow,AskClose,PipsHigh,PipsLow,PipsRange,PipsHighBid,PipsLowBid,PipsRangeBid,PipsHighAsk,PipsLowAsk,PipsRangeAsk";
        }

        // overrides
        public override string ToString()
        {
            return string.Format("{0} begin={1} end={2} O={3} H={4} L={5} C={6} PR={7} PH={8} PL={9} ID={10}",
                Symbol, DateTimeBegin, DateTimeEnd, Open, High, Low, Close, PipsRange, PipsHigh, PipsLow, SymbolId);
        }

    }
}

