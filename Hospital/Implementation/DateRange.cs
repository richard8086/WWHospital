using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Implementation
{

    public struct DateRange : IComparable<DateRange>
    {
        public Date Start { get; set; }
        public Date End { get; set; }

        public DateRange(Date start, Date end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// compare the start value only
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(DateRange other)
        {
            return Start.Value.CompareTo(other.Start.Value);
        }

        /// <summary>
        /// test if the ranges overlap and returns the earliest overlapping date     
        /// </summary>
        /// <param name="range"></param>
        /// <returns>boolean indicates overlapping or not, Date indicates the earliest overlapping date</returns>
        public Tuple<bool, Date> Overlap(DateRange range)
        {
            if (Start.Value >= range.Start.Value &&
                Start.Value <= range.End.Value)
            {
                return new Tuple<bool, Date>(true, Start);
            }

            if (range.Start.Value >= Start &&
                range.Start.Value <= End)
            {
                return new Tuple<bool, Date>(true, range.Start);
            }

            return new Tuple<bool, Date>(false, DateTime.MinValue);
        }
    }

}