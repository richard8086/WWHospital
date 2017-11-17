using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.Interfaces
{
    public interface IUtility
    {
        /// <summary>
        /// merge the sorted addedlist to the sorted baselist. The two lists are sorted on ascending order based on start date. 
        /// </summary>
        /// <param name="baseList"></param>
        /// <param name="addedList"></param>
        void MergeIntervalList(LinkedList<Tuple<DateRange, object>> baseList, LinkedList<Tuple<DateRange, object>> addedList);

        /// <summary>
        /// Find the earliest DateRange that overlaps and return the earliest date
        /// </summary>
        /// <param name="list1">sorted list</param>
        /// <param name="list2">sorted list</param>
        /// <returns>boolean indicates success, earliest date that the two interval lists overlap</returns>
        Tuple<bool, Date, object, object> FindFirstOverlappingDate(LinkedList<Tuple<DateRange, object>> list1, LinkedList<Tuple<DateRange, object>> list2);
    }
}
