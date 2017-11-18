using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public struct Date : IComparable<Date>, IEquatable<Date>
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

        public bool Equals(Date other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var b2 = (Date)obj;
            return CompareTo(b2) == 0;
        }

        public override int GetHashCode()
        {
            return _date.GetHashCode();
        }

        public static bool operator ==(Date obj1, Date obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Date obj1, Date obj2)
        {
            return !obj1.Equals(obj2);
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