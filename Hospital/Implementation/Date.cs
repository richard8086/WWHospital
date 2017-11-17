using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Implementation
{
    public struct Date : IComparable<Date>
    {
        public DateTime Value
        {
            get { return _date; }
            set { _date = value.Date; }
        }

        public Date(DateTime dateTime)
        {
            _date = dateTime.Date;
        }

        public int CompareTo(Date other)
        {
            return _date.CompareTo(other._date);
        }

        // User-defined conversion from Digit to double
        public static implicit operator DateTime(Date d)
        {
            return d._date;
        }
        //  User-defined conversion from double to Digit
        public static implicit operator Date(DateTime d)
        {
            return new Date(d);
        }


        private DateTime _date;

    }
}