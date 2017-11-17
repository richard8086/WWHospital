using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Implementation;

namespace Hospital.Tests.Implementation
{
    [TestClass]
    public class DateTest
    {
        [TestMethod]
        public void CompareDateEqualTimeNotEqual()
        {
            var d1 = new Date(new DateTime(2000, 1, 1, 1, 1, 1));
            var d2 = new Date(new DateTime(2000, 1, 1, 2, 2, 2));
            Assert.IsTrue(d1.CompareTo(d2) == 0);
        }

        [TestMethod]
        public void CompareDateLess()
        {
            var d1 = new Date(new DateTime(2000, 1, 1, 1, 1, 1));
            var d2 = new Date(new DateTime(2000, 1, 2, 2, 2, 2));
            Assert.IsTrue(d1.CompareTo(d2) < 0);
        }

        [TestMethod]
        public void CompareDateGreater()
        {
            var d1 = new Date(new DateTime(2000, 2, 1, 1, 1, 1));
            var d2 = new Date(new DateTime(2000, 1, 2, 2, 2, 2));
            Assert.IsTrue(d1.CompareTo(d2) > 0);
        }
    }
}
