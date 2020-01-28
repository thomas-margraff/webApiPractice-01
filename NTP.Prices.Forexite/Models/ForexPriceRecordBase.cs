using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForexDomainObject
{
    public class ForexPriceRecordBase
    {
        private double _open;
        private double _high;
        private double _low;
        private double _close;

        public int SymbolId { get; set; }
        public string Symbol { get; set; }
        public double Open
        {
            get
            {
                if (this._open == 0)
                    return this.MidOpen;
                return _open;
            }
            set { _open = value; }
        }
        public double High
        {
            get
            {
                if (this._high == 0)
                    return this.MidHigh;
                return _high;
            }
            set { _high = value; }
        }
        public double Low
        {
            get
            {
                if (this._low == 0)
                    return this.MidLow;
                return _low;
            }
            set { _low = value; }
        }
        public double Close
        {
            get
            {
                if (this._close == 0)
                    return this.MidClose;
                return _close;
            }
            set { _close = value; }
        }
        public double BidOpen { get; set; }
        public double BidHigh { get; set; }
        public double BidLow { get; set; }
        public double BidClose { get; set; }
        public double AskOpen { get; set; }
        public double AskHigh { get; set; }
        public double AskLow { get; set; }
        public double AskClose { get; set; }
        public double PipMultiplier
        {
            get { return _mult; }
        }

        private double _mult
        {
            get
            {
                // gold and silver
                if (Symbol.ToLower().StartsWith("xau") || Symbol.ToLower().StartsWith("xag"))
                    return 10;

                // all except jpy cross
                if (Symbol.ToLower().IndexOf("jpy") > 0)
                    return 100;

                // jpy cross
                return 10000;
            }
        }
        private double calcMid(double m)
        {
            double r = 0.0;
            if (_mult >= 100)
                r = (int)Math.Round(m * _mult);
            else
                r = Math.Round(m, 2);
            return r / 2;
        }
        
        public double MidOpen
        {
            get { return Math.Round(this.BidOpen + ((this.AskOpen - this.BidOpen) / 2), 5); }
        }
        public double MidHigh
        {
            get { return Math.Round(this.BidHigh + ((this.AskHigh - this.BidHigh) / 2), 5); }
        }
        public double MidLow
        {
            get { return Math.Round(this.BidLow + ((this.AskLow - this.BidLow) / 2), 5); }
        }
        public double MidClose
        {
            get { return Math.Round(this.BidClose + ((this.AskClose - this.BidClose) / 2), 5); }
        }

        #region pips range
        public double PipsRange
        {
            get
            {
                double h = this.High - this.Open;
                double l = this.Low - this.Open;
                return calcRange(h, l);
            }
        }
        public double PipsRangeBid
        {
            get
            {
                double h = this.BidHigh - this.BidOpen;
                double l = this.BidLow - this.BidOpen;

                return calcRange(h, l);
            }
        }
        public double PipsRangeAsk
        {
            get
            {
                double h = this.AskHigh - this.AskOpen;
                double l = this.AskLow - this.AskOpen;

                return calcRange(h, l);
            }
        }
        private double calcRange(double h, double l)
        {
            double r = 0.0;

            if (_mult >= 100)
                r = (int)Math.Round((h - l) * _mult);
            else
                r = Math.Round((h - l), 2);

            return r;
        }
        #endregion

        #region pips high
        private double calcHigh(double h)
        {
            double r = 0.0;
            if (_mult >= 100)
                r = (int)Math.Round(h * _mult);
            else
                r = Math.Round(h, 2);
            return r;
        }
        public double PipsHigh
        {
            get { return calcHigh(this.High - this.Open); }
        }
        public double PipsHighBid
        {
            get { return calcHigh(this.BidHigh - this.BidOpen); }
        }
        public double PipsHighAsk
        {
            get { return calcHigh(this.AskHigh - this.AskOpen); }
        }
        #endregion

        #region pips low
        private double calcLow(double l)
        {
            double r = 0.0;

            if (_mult >= 100)
                r = (int)Math.Round(l * _mult);
            else
                r = Math.Round(l, 2);

            return r;

        }
        public double PipsLow
        {
            get { return calcLow(this.Low - this.Open); }
        }
        public double PipsLowBid
        {
            get { return calcLow(this.BidLow - this.BidOpen); }
        }
        public double PipsLowAsk
        {
            get { return calcLow(this.AskLow - this.AskOpen); }
        }
        #endregion

        public ForexPriceRecordBase() { }

        public string ToCsv()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
               SymbolId, Symbol, Open, High, Low, Close, BidOpen, BidHigh, BidLow, BidClose, AskOpen, AskHigh, AskLow, AskClose);
        }
    }

}
