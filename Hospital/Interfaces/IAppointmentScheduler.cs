using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hospital.Models;

namespace Hospital.Interfaces
{
    /// <summary>
    /// Appointment scheduler interface
    /// </summary>
    public interface IAppointmentScheduler
    {
        /// <summary>
        /// Initialize with a user object
        /// </summary>
        /// <param name="userData"></param>
        void Initialize(object userData);

        /// <summary>
        /// Book an appointment
        /// </summary>
        /// <param name="date"></param>
        /// <returns>true if successful</returns>
        bool Book(Date date);

        /// <summary>
        /// Cancel an appointment
        /// </summary>
        /// <param name="date"></param>
        void Cancel(Date date);

        /// <summary>
        /// Retrieve all the available appointment intervals in the form of sorted DateRanges plus user data
        /// </summary>
        /// <returns>Sorted on ascending order date ranges plus user object </returns>
        LinkedList<Tuple<DateRange, object> > GetAvailableIntervals();

        /// <summary>
        /// Get the existing appointments
        /// </summary>
        /// <returns></returns>
        List<DateTime> GetBookings();

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <returns></returns>
        IAppointmentScheduler Create();
    }
}