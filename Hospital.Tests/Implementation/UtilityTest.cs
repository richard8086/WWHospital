using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Implementation;
using Hospital.Models;
using Hospital.Interfaces;
using System.Collections.Generic;

namespace Hospital.Tests.Implementation
{
    [TestClass]
    public class UtilityTest
    {
        [TestMethod]
        public void TestMergeToEmpty()
        {
            var d1 = new DateRange(DateTime.Now + TimeSpan.FromDays(1), DateTime.MaxValue);
            var d2 = new DateRange(DateTime.Now + TimeSpan.FromDays(2), DateTime.MaxValue);
            var d4 = new DateRange(DateTime.Now + TimeSpan.FromDays(4), DateTime.MaxValue);
            var d5 = new DateRange(DateTime.Now + TimeSpan.FromDays(5), DateTime.MaxValue);
            var d7 = new DateRange(DateTime.Now + TimeSpan.FromDays(7), DateTime.MaxValue);

            IUtility util = new Utility();
            LinkedList<Tuple<DateRange, object>> list1 = new LinkedList<Tuple<DateRange, object>>();
            LinkedList<Tuple<DateRange, object>> list2 = new LinkedList<Tuple<DateRange, object>>();

            list2.AddLast(new Tuple<DateRange, object>(d1, null));
            list2.AddLast(new Tuple<DateRange, object>(d2, null));
            list2.AddLast(new Tuple<DateRange, object>(d4, null));
            list2.AddLast(new Tuple<DateRange, object>(d5, null));
            list2.AddLast(new Tuple<DateRange, object>(d7, null));

            util.MergeIntervalList(list1, list2);
            var merged = new List<Tuple<DateRange, object>>(list1);
            Assert.AreEqual<int>(merged.Count, 5);
            Assert.IsTrue(merged[0].Item1.CompareTo(d1) == 0);
            Assert.IsTrue(merged[1].Item1.CompareTo(d2) == 0);
            Assert.IsTrue(merged[2].Item1.CompareTo(d4) == 0);
            Assert.IsTrue(merged[3].Item1.CompareTo(d5) == 0);
            Assert.IsTrue(merged[4].Item1.CompareTo(d7) == 0);
        }

        [TestMethod]
        public void TestMergeFromEmpty()
        {
            var d1 = new DateRange(DateTime.Now + TimeSpan.FromDays(1), DateTime.MaxValue);
            var d2 = new DateRange(DateTime.Now + TimeSpan.FromDays(2), DateTime.MaxValue);
            var d4 = new DateRange(DateTime.Now + TimeSpan.FromDays(4), DateTime.MaxValue);
            var d5 = new DateRange(DateTime.Now + TimeSpan.FromDays(5), DateTime.MaxValue);
            var d7 = new DateRange(DateTime.Now + TimeSpan.FromDays(7), DateTime.MaxValue);

            IUtility util = new Utility();
            LinkedList<Tuple<DateRange, object>> list1 = new LinkedList<Tuple<DateRange, object>>();
            LinkedList<Tuple<DateRange, object>> list2 = new LinkedList<Tuple<DateRange, object>>();

            list2.AddLast(new Tuple<DateRange, object>(d1, null));
            list2.AddLast(new Tuple<DateRange, object>(d2, null));
            list2.AddLast(new Tuple<DateRange, object>(d4, null));
            list2.AddLast(new Tuple<DateRange, object>(d5, null));
            list2.AddLast(new Tuple<DateRange, object>(d7, null));

            util.MergeIntervalList(list2, list1);
            var merged = new List<Tuple<DateRange, object>>(list2);
            Assert.AreEqual<int>(merged.Count, 5);
            Assert.IsTrue(merged[0].Item1.CompareTo(d1) == 0);
            Assert.IsTrue(merged[1].Item1.CompareTo(d2) == 0);
            Assert.IsTrue(merged[2].Item1.CompareTo(d4) == 0);
            Assert.IsTrue(merged[3].Item1.CompareTo(d5) == 0);
            Assert.IsTrue(merged[4].Item1.CompareTo(d7) == 0);
        }

        [TestMethod]
        public void TestInterleaved()
        {
            var d1 = new DateRange(DateTime.Now + TimeSpan.FromDays(1), DateTime.MaxValue);
            var d2 = new DateRange(DateTime.Now + TimeSpan.FromDays(2), DateTime.MaxValue);
            var d4 = new DateRange(DateTime.Now + TimeSpan.FromDays(4), DateTime.MaxValue);
            var d5 = new DateRange(DateTime.Now + TimeSpan.FromDays(5), DateTime.MaxValue);
            var d7 = new DateRange(DateTime.Now + TimeSpan.FromDays(7), DateTime.MaxValue);

            IUtility util = new Utility();
            LinkedList<Tuple<DateRange, object>> list1 = new LinkedList<Tuple<DateRange, object>>();
            LinkedList<Tuple<DateRange, object>> list2 = new LinkedList<Tuple<DateRange, object>>();

            list1.AddLast(new Tuple<DateRange, object>(d1, null));
            list1.AddLast(new Tuple<DateRange, object>(d4, null));
            list1.AddLast(new Tuple<DateRange, object>(d5, null));

            list2.AddLast(new Tuple<DateRange, object>(d2, null));
            list2.AddLast(new Tuple<DateRange, object>(d7, null));

            util.MergeIntervalList(list1, list2);
            var merged = new List<Tuple<DateRange, object>>(list1);
            Assert.AreEqual<int>(merged.Count, 5);
            Assert.IsTrue(merged[0].Item1.CompareTo(d1) == 0);
            Assert.IsTrue(merged[1].Item1.CompareTo(d2) == 0);
            Assert.IsTrue(merged[2].Item1.CompareTo(d4) == 0);
            Assert.IsTrue(merged[3].Item1.CompareTo(d5) == 0);
            Assert.IsTrue(merged[4].Item1.CompareTo(d7) == 0);
        }

        [TestMethod]
        public void TestInterleavedAnother()
        {
            var d1 = new DateRange(DateTime.Now + TimeSpan.FromDays(1), DateTime.MaxValue);
            var d2 = new DateRange(DateTime.Now + TimeSpan.FromDays(2), DateTime.MaxValue);
            var d4 = new DateRange(DateTime.Now + TimeSpan.FromDays(4), DateTime.MaxValue);
            var d5 = new DateRange(DateTime.Now + TimeSpan.FromDays(5), DateTime.MaxValue);
            var d7 = new DateRange(DateTime.Now + TimeSpan.FromDays(7), DateTime.MaxValue);

            IUtility util = new Utility();
            LinkedList<Tuple<DateRange, object>> list1 = new LinkedList<Tuple<DateRange, object>>();
            LinkedList<Tuple<DateRange, object>> list2 = new LinkedList<Tuple<DateRange, object>>();

            list1.AddLast(new Tuple<DateRange, object>(d1, null));
            list1.AddLast(new Tuple<DateRange, object>(d4, null));
            list1.AddLast(new Tuple<DateRange, object>(d5, null));

            list2.AddLast(new Tuple<DateRange, object>(d2, null));
            list2.AddLast(new Tuple<DateRange, object>(d7, null));

            util.MergeIntervalList(list2, list1);
            var merged = new List<Tuple<DateRange, object>>(list2);
            Assert.AreEqual<int>(merged.Count, 5);
            Assert.IsTrue(merged[0].Item1.CompareTo(d1) == 0);
            Assert.IsTrue(merged[1].Item1.CompareTo(d2) == 0);
            Assert.IsTrue(merged[2].Item1.CompareTo(d4) == 0);
            Assert.IsTrue(merged[3].Item1.CompareTo(d5) == 0);
            Assert.IsTrue(merged[4].Item1.CompareTo(d7) == 0);
        }

        [TestMethod]
        public void FindOverlapping1()
        {
            List<Date> dates = new List<Date>();
            for (int i = 0; i < 10; i++)
            {
                dates.Add(DateTime.Now + TimeSpan.FromDays(i));
            }

            IUtility util = new Utility();
            LinkedList<Tuple<DateRange, object>> list1 = new LinkedList<Tuple<DateRange, object>>();
            LinkedList<Tuple<DateRange, object>> list2 = new LinkedList<Tuple<DateRange, object>>();

            list1.AddLast(new Tuple<DateRange, object>(new DateRange(dates[0], dates[2]), null));
            list1.AddLast(new Tuple<DateRange, object>(new DateRange(dates[5], dates[7]), null));

            list2.AddLast(new Tuple<DateRange, object>(new DateRange(dates[3], dates[3]), null));
            list2.AddLast(new Tuple<DateRange, object>(new DateRange(dates[4], dates[5]), null));

            var result = util.FindFirstOverlappingDate(list2, list1);
            Assert.IsTrue(result.Item1);
            Assert.IsTrue(result.Item2.Value.CompareTo(dates[5]) == 0);
        }

        [TestMethod]
        public void FindOverlapping2()
        {
            List<Date> dates = new List<Date>();
            for (int i = 0; i < 10; i++)
            {
                dates.Add(DateTime.Now + TimeSpan.FromDays(i));
            }

            IUtility util = new Utility();
            LinkedList<Tuple<DateRange, object>> list1 = new LinkedList<Tuple<DateRange, object>>();
            LinkedList<Tuple<DateRange, object>> list2 = new LinkedList<Tuple<DateRange, object>>();

            list1.AddLast(new Tuple<DateRange, object>(new DateRange(dates[0], dates[2]), null));
            list1.AddLast(new Tuple<DateRange, object>(new DateRange(dates[5], dates[7]), null));

            list2.AddLast(new Tuple<DateRange, object>(new DateRange(dates[0], dates[3]), null));
            list2.AddLast(new Tuple<DateRange, object>(new DateRange(dates[4], dates[5]), null));

            var result = util.FindFirstOverlappingDate(list2, list1);

            Assert.IsTrue(result.Item1);
            Assert.IsTrue(result.Item2.Value.CompareTo(dates[0]) == 0);
        }

        [TestMethod]
        public void FindOverlapping13()
        {
            List<Date> dates = new List<Date>();
            for (int i = 0; i < 10; i++)
            {
                dates.Add(DateTime.Now + TimeSpan.FromDays(i));
            }

            IUtility util = new Utility();
            LinkedList<Tuple<DateRange, object>> list1 = new LinkedList<Tuple<DateRange, object>>();
            LinkedList<Tuple<DateRange, object>> list2 = new LinkedList<Tuple<DateRange, object>>();

            list1.AddLast(new Tuple<DateRange, object>(new DateRange(dates[0], dates[2]), null));
            list1.AddLast(new Tuple<DateRange, object>(new DateRange(dates[5], dates[7]), null));

            list2.AddLast(new Tuple<DateRange, object>(new DateRange(dates[1], dates[3]), null));
            list2.AddLast(new Tuple<DateRange, object>(new DateRange(dates[4], dates[5]), null));

            var result = util.FindFirstOverlappingDate(list2, list1);
            Assert.IsTrue(result.Item1);
            Assert.IsTrue(result.Item2.Value.CompareTo(dates[1]) == 0);
        }
    }
}
