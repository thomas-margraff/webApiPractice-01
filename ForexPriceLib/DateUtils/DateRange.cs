using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ForexPriceLib.DateUtils
{
    /// <summary>
    /// DateRange Class
    /// </summary>
    public class DateRange : IEnumerable<DateTime>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        /// <summary>
        /// StartDate Id
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// EndDate Id
        /// </summary>
        public DateTime EndDate { get; set; }
        public IEnumerator<DateTime> GetEnumerator()
        {
            return new DateRangeEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    /// <summary>
    /// DateRangeEnumerator Class
    /// </summary>
    public class DateRangeEnumerator : IEnumerator<DateTime>
    {
        private int _index = -1;
        private readonly DateRange _dateRange;
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="dateRange"></param>
        public DateRangeEnumerator(DateRange dateRange)
        {
            _dateRange = dateRange;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { }
        /// <summary>
        /// MoveNext
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            _index++;

            // my mod 2/7/2010
            if (_index > (_dateRange.EndDate.Date - _dateRange.StartDate.Date).Days)
                return false;

            // orig
            //if (_index > (_dateRange.EndDate - _dateRange.StartDate).Days)            
            //    return false; 
            return true;
        }
        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            _index = -1;
        }
        /// <summary>
        /// Current Item
        /// </summary>
        public DateTime Current
        {
            get { return _dateRange.StartDate.AddDays(_index); }
        }
        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

}
