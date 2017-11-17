using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hospital.Implementation;

namespace Hospital.Tests.Implementation
{
    [TestClass]
    public class SchedulerTest
    {
        [TestMethod]
        public void TestToday()
        {
            IAppointmentScheduler scheduler = new AppointmentScheduler();
            DateTime today = DateTime.Now;
            Assert.IsFalse(scheduler.Book(today));
        }

        [TestMethod]
        public void TestTomorrow()
        {
            IAppointmentScheduler scheduler = new AppointmentScheduler();
            DateTime tomorrow = DateTime.Now + TimeSpan.FromDays(1);
            Assert.IsTrue(scheduler.Book(tomorrow));
            Assert.IsFalse(scheduler.Book(tomorrow));
            scheduler.Cancel(tomorrow);
            Assert.IsTrue(scheduler.Book(tomorrow));
        }

        [TestMethod]
        public void TestDay1245()
        {
            IAppointmentScheduler scheduler = new AppointmentScheduler();
            DateTime day1 = DateTime.Now + TimeSpan.FromDays(1);
            Assert.IsTrue(scheduler.Book(day1));
            DateTime day2 = DateTime.Now + TimeSpan.FromDays(2);
            Assert.IsTrue(scheduler.Book(day2));
            DateTime day4 = DateTime.Now + TimeSpan.FromDays(4);
            Assert.IsTrue(scheduler.Book(day4));
            DateTime day5 = DateTime.Now + TimeSpan.FromDays(5);
            Assert.IsTrue(scheduler.Book(day5));

            Assert.IsFalse(scheduler.Book(day1));
            Assert.IsFalse(scheduler.Book(day2));
            Assert.IsFalse(scheduler.Book(day4));
            Assert.IsFalse(scheduler.Book(day5));

            scheduler.Cancel(day1);
            scheduler.Cancel(day2);
            scheduler.Cancel(day4);
            scheduler.Cancel(day5);

            Assert.IsTrue(scheduler.GetBookings().Count == 0);
            var avail = scheduler.GetAvailableIntervals();
            Assert.IsTrue(avail.First != null);
            Assert.IsTrue(avail.First.Next == null);
            Assert.IsTrue(avail.First.Value.Item1.CompareTo(new DateRange(day1, DateTime.MaxValue)) == 0);
        }

        [TestMethod]
        public void TestDay5421()
        {
            IAppointmentScheduler scheduler = new AppointmentScheduler();

            DateTime day5 = DateTime.Now + TimeSpan.FromDays(5);
            Assert.IsTrue(scheduler.Book(day5));
            DateTime day4 = DateTime.Now + TimeSpan.FromDays(4);
            Assert.IsTrue(scheduler.Book(day4));
            DateTime day2 = DateTime.Now + TimeSpan.FromDays(2);
            Assert.IsTrue(scheduler.Book(day2));
            DateTime day1 = DateTime.Now + TimeSpan.FromDays(1);
            Assert.IsTrue(scheduler.Book(day1));

            Assert.IsFalse(scheduler.Book(day1));
            Assert.IsFalse(scheduler.Book(day2));
            Assert.IsFalse(scheduler.Book(day4));
            Assert.IsFalse(scheduler.Book(day5));


            scheduler.Cancel(day2);
            scheduler.Cancel(day4);
            scheduler.Cancel(day1);
            scheduler.Cancel(day5);

            Assert.IsTrue(scheduler.GetBookings().Count == 0);
            var avail = scheduler.GetAvailableIntervals();
            Assert.IsTrue(avail.First != null);
            Assert.IsTrue(avail.First.Next == null);
            Assert.IsTrue(avail.First.Value.Item1.CompareTo(new DateRange(day1, DateTime.MaxValue)) == 0);
        }

        [TestMethod]
        public void TestDay123()
        {
            IAppointmentScheduler scheduler = new AppointmentScheduler();
            DateTime day1 = DateTime.Now + TimeSpan.FromDays(1);
            Assert.IsTrue(scheduler.Book(day1));
            DateTime day2 = DateTime.Now + TimeSpan.FromDays(2);
            Assert.IsTrue(scheduler.Book(day2));
            DateTime day3 = DateTime.Now + TimeSpan.FromDays(3);
            Assert.IsTrue(scheduler.Book(day3));


            Assert.IsTrue(scheduler.GetBookings().Count == 3);
            var avail1 = scheduler.GetAvailableIntervals();
            Assert.IsTrue(avail1.First != null);
            Assert.IsTrue(avail1.First.Next == null);
            DateTime day4 = DateTime.Now + TimeSpan.FromDays(4);
            Assert.IsTrue(avail1.First.Value.Item1.CompareTo(new DateRange(day4, DateTime.MaxValue)) == 0);


            Assert.IsFalse(scheduler.Book(day1));
            Assert.IsFalse(scheduler.Book(day2));
            Assert.IsFalse(scheduler.Book(day3));

            scheduler.Cancel(day2);
            scheduler.Cancel(day3);
            scheduler.Cancel(day1);

            Assert.IsTrue(scheduler.GetBookings().Count == 0);
            var avail2 = scheduler.GetAvailableIntervals();
            Assert.IsTrue(avail2.First != null);
            Assert.IsTrue(avail2.First.Next == null);
            Assert.IsTrue(avail2.First.Value.Item1.CompareTo(new DateRange(day1, DateTime.MaxValue)) == 0);
        }
    }
}
