using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Implementation;

namespace Hospital.Tests.Implementation
{
    [TestClass]
    public class DateRangeTest
    {
        [TestMethod]
        public void CompareEqual()
        {
            var d1x = new Date(new DateTime(2000, 1, 1));
            var d1y = new Date(new DateTime(2000, 1, 5));
            var d1r = new DateRange(d1x, d1y);


            var d2x = new Date(new DateTime(2000, 1, 1));
            var d2y = new Date(new DateTime(2000, 1, 2));
            var d2r = new DateRange(d2x, d2y);

            Assert.IsTrue(d1r.CompareTo(d2r) == 0);
        }

        [TestMethod]
        public void CompareLess()
        {
            var d1x = new Date(new DateTime(2000, 1, 1));
            var d1y = new Date(new DateTime(2000, 1, 5));
            var d1r = new DateRange(d1x, d1y);


            var d2x = new Date(new DateTime(2000, 1, 2));
            var d2y = new Date(new DateTime(2000, 1, 2));
            var d2r = new DateRange(d2x, d2y);

            Assert.IsTrue(d1r.CompareTo(d2r) < 0);
        }

        [TestMethod]
        public void CompareGreater()
        {
            var d1x = new Date(new DateTime(2000, 1, 3));
            var d1y = new Date(new DateTime(2000, 1, 5));
            var d1r = new DateRange(d1x, d1y);


            var d2x = new Date(new DateTime(2000, 1, 2));
            var d2y = new Date(new DateTime(2000, 1, 2));
            var d2r = new DateRange(d2x, d2y);

            Assert.IsTrue(d1r.CompareTo(d2r) > 0);
        }

        [TestMethod]
        public void Overlap1()
        {
            var d1x = new Date(new DateTime(2000, 1, 1));
            var d1y = new Date(new DateTime(2000, 1, 5));
            var d1r = new DateRange(d1x, d1y);


            var d2x = new Date(new DateTime(2000, 1, 2));
            var d2y = new Date(new DateTime(2000, 1, 3));
            var d2r = new DateRange(d2x, d2y);

            Assert.IsTrue(d1r.Overlap(d2r).Item1);
            Assert.IsTrue(d1r.Overlap(d2r).Item2.CompareTo(d2x) == 0);
        }

        [TestMethod]
        public void Overlap2()
        {
            var d1x = new Date(new DateTime(2000, 1, 1));
            var d1y = new Date(new DateTime(2000, 1, 5));
            var d1r = new DateRange(d1x, d1y);


            var d2x = new Date(new DateTime(2000, 1, 1));
            var d2y = new Date(new DateTime(2000, 1, 3));
            var d2r = new DateRange(d2x, d2y);

            Assert.IsTrue(d1r.Overlap(d2r).Item1);
            Assert.IsTrue(d1r.Overlap(d2r).Item2.CompareTo(d2x) == 0);
        }

        [TestMethod]
        public void Overlap3()
        {
            var d1x = new Date(new DateTime(2000, 1, 2));
            var d1y = new Date(new DateTime(2000, 1, 3));
            var d1r = new DateRange(d1x, d1y);


            var d2x = new Date(new DateTime(2000, 1, 1));
            var d2y = new Date(new DateTime(2000, 1, 3));
            var d2r = new DateRange(d2x, d2y);

            Assert.IsTrue(d1r.Overlap(d2r).Item1);
            Assert.IsTrue(d1r.Overlap(d2r).Item2.CompareTo(d1x) == 0);
        }


        [TestMethod]
        public void OverlapNot()
        {
            var d1x = new Date(new DateTime(2000, 1, 2));
            var d1y = new Date(new DateTime(2000, 1, 3));
            var d1r = new DateRange(d1x, d1y);


            var d2x = new Date(new DateTime(2000, 1, 4));
            var d2y = new Date(new DateTime(2000, 1, 5));
            var d2r = new DateRange(d2x, d2y);

            Assert.IsFalse(d1r.Overlap(d2r).Item1);
        }

    }
}
