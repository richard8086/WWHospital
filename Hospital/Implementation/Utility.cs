using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Hospital.Interfaces;
using Hospital.Models;

namespace Hospital.Implementation
{
    public class Utility : IUtility
    {
        /// <summary>
        /// Find the earliest DateRange that overlaps and return the earliest date
        /// </summary>
        /// <param name="list1">sorted list</param>
        /// <param name="list2">sorted list</param>
        /// <returns>earliest date that the two interval lists overlap</returns>
        Tuple<bool, Date, object, object> IUtility.FindFirstOverlappingDate(LinkedList<Tuple<DateRange, object>> list1, LinkedList<Tuple<DateRange, object>> list2)
        {
            var result = new Tuple<Date, object, object>(DateTime.MinValue, null, null);

            var node1 = list1.First;
            var node2 = list2.First;
            while (node1 != null && node2 != null)
            {
                var overlap = node1.Value.Item1.Overlap(node2.Value.Item1);
                if (overlap.Item1)
                {
                    return new Tuple<bool, Date, object, object>(true, overlap.Item2, node1.Value.Item2, node2.Value.Item2);
                }

                if (node1.Value.Item1.CompareTo(node2.Value.Item1) < 0)
                {
                    node1 = node1.Next;
                }
                else
                {
                    Debug.Assert(node1.Value.Item1.CompareTo(node2.Value.Item1) > 0);
                    node2 = node2.Next;
                }
            }

            return new Tuple<bool, Date, object, object>(false, DateTime.MaxValue, null, null);
        }


        /// <summary>
        /// merge the sorted addedlist to the sorted base list. The two lists are sorted on ascending order based on start date. o(n), n being the combined list size
        /// </summary>
        /// <param name="baseList"></param>
        /// <param name="addedList"></param>
        void IUtility.MergeIntervalList(LinkedList<Tuple<DateRange, object>> baseList, LinkedList<Tuple<DateRange, object>> addedList)
        {
            var baseNode = baseList.First;
            var addedNode = addedList.First;

            // merge
            while (addedNode != null && baseNode != null)
            {
                if (baseNode.Value.Item1.CompareTo(addedNode.Value.Item1) > 0)
                {
                    baseList.AddBefore(baseNode, addedNode.Value);
                    addedNode = addedNode.Next;
                }
                else
                {
                    baseNode = baseNode.Next;
                }
            }

            // add the remaining nodes
            while (addedNode != null)
            {
                baseList.AddLast(addedNode.Value);
                addedNode = addedNode.Next;
            }
        }
    }
}