using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Hospital.Interfaces;
using Hospital.Models;

namespace Hospital.Implementation
{
    /// <summary>
    /// Appointment scheduler with sorted linked list available intervals and sorted existing appointments
    /// </summary>
    public class AppointmentScheduler : IAppointmentScheduler
    {

        /// <summary>
        /// Initialize with a user object
        /// </summary>
        /// <param name="userData"></param>
        void IAppointmentScheduler.Initialize(object userData)
        {
            _userData = userData;
        }

        /// <summary>
        /// Book an appointment. o(n), n being the number of intervals
        /// </summary>
        /// <param name="date"></param>
        /// <returns>true if successful</returns>
        bool IAppointmentScheduler.Book(Date date)
        {
            if (_existing.Contains(date))
            {
                return false;
            }

            // skip all the intervals that precedes the date
            var current = _avail.First;
            while (current != null && current.Value.End.Value < date.Value)
            {
                current = current.Next;
            }

            // the date is in the range, added the date
            if (current != null && current.Value.Start.Value <= date.Value)
            {
                Add(current, date);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Cancel an appointment. o(n), n being the number of intervals
        /// </summary>
        /// <param name="date"></param>
        void IAppointmentScheduler.Cancel(Date date)
        {
            if (!_existing.Contains(date))
            {
                return;
            }

            var current = _avail.First;
            // skip the range's start date less than the date
            while (current != null && current.Value.Start.Value < date.Value)
            {
                current = current.Next;
            }

            // Since the last node start value must be greater than date, this can not happen
            if (current == null)
            {
                Debug.Assert(false);
                return;
            }

            // now we are at the first node that is at the right of the date
            Debug.Assert(current.Value.Start.Value > date.Value);

            var previous = current.Previous;
            if (date.Value + TimeSpan.FromDays(1) == current.Value.Start.Value &&
                previous != null && previous.Value.End.Value + TimeSpan.FromDays(1) == date.Value)  // date connecting the previous node and current node
            {
                DateRange range = new DateRange(previous.Value.Start, current.Value.End);
                _avail.AddBefore(previous, range);
                _avail.Remove(current);
                _avail.Remove(previous);
            }
            else if (previous != null && previous.Value.End.Value + TimeSpan.FromDays(1) == date.Value) // date connecting the previous node
            {
                DateRange range = new DateRange(previous.Value.Start, date);
                _avail.AddBefore(previous, range);
                _avail.Remove(previous);
            }
            else if (date.Value + TimeSpan.FromDays(1) == current.Value.Start.Value) // date connecting the current node
            {
                DateRange range = new DateRange(date, current.Value.End);
                _avail.AddBefore(current, range);
                _avail.Remove(current);
            }
            else  // // date not connecting the previous node or current node
            {
                DateRange range = new DateRange(date, date);
                _avail.AddBefore(current, range);
            }

            // remove from the existing list
            _existing.Remove(date);
        }

        /// <summary>
        /// Retrieve all the available appointment intervals in the form of sorted DateRanges plus user data. o(n), n being the number of intervals
        /// </summary>
        /// <returns>Sorted on ascending order date ranges plus user object </returns>
        LinkedList<Tuple<DateRange, object>> IAppointmentScheduler.GetAvailableIntervals()
        {
            LinkedList<Tuple<DateRange, object>> list = new LinkedList<Tuple<DateRange, object>>();

            foreach (var range in _avail)
            {
                list.AddLast(new Tuple<DateRange, object>(range, _userData));
            }

            return list;
        }


        /// <summary>
        /// Get the existing appointments. o(n), n being the number of existing appoints
        /// </summary>
        /// <returns></returns>
        List<DateTime> IAppointmentScheduler.GetBookings()
        {
            var list = new List<DateTime>();
            foreach (var date in _existing)
            {
                list.Add(date);
            }
            return list;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AppointmentScheduler()
        {
            // init with the range from tomorrow till max date
            _avail.AddFirst(new DateRange(DateTime.Now + TimeSpan.FromDays(1), DateTime.MaxValue));
        }

        /// <summary>
        /// Add the date to existing the appointment list
        /// </summary>
        /// <param name="current"></param>
        /// <param name="date"></param>
        private void Add(LinkedListNode<DateRange> current, Date date)
        {
            Debug.Assert(!_existing.Contains(date));

            Debug.Assert(current.Value.Start.Value <= date.Value);
            Debug.Assert(current.Value.End.Value >= date.Value);

            // break up the current range to the left and right
            DateRange leftRange = new DateRange(current.Value.Start, date.Value - TimeSpan.FromDays(1));
            if (leftRange.Start.Value <= leftRange.End.Value)
            {
                _avail.AddBefore(current, leftRange);

            }

            DateRange rightRange = new DateRange(date.Value + TimeSpan.FromDays(1), current.Value.End);
            if (rightRange.Start.Value <= rightRange.End.Value)
            {
                _avail.AddAfter(current, rightRange);

            }
            // remove the current
            _avail.Remove(current);

            // add to the existing list
            _existing.Add(date);
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <returns></returns>
        IAppointmentScheduler IAppointmentScheduler.Create()
        {
            return new AppointmentScheduler();
        }

        /// <summary>
        /// Sorted available date ranges
        /// </summary>
        private LinkedList<DateRange> _avail = new LinkedList<DateRange>(); 
        /// <summary>
        /// Sorted existing appointments
        /// </summary>
        private SortedSet<Date> _existing = new SortedSet<Date>();

        /// <summary>
        /// user data
        /// </summary>
        private object _userData = null;
    }
}