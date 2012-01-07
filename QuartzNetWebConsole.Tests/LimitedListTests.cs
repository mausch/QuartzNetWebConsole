using System;
using System.Linq;
using NUnit.Framework;
using QuartzNetWebConsole.Utils;

namespace QuartzNetWebConsole.Tests {
    public static class LimitedListTests {
        public static void tt() {
            var b = new LimitedList<int>(5);
            foreach (var i in Enumerable.Range(0, 1000))
                b.Add(i);
            var a = b.ToArray();
            Assert.AreEqual(5, a.Length);
            Assert.AreEqual(999, a[0]);
            Assert.AreEqual(998, a[1]);
            Assert.AreEqual(997, a[2]);
            Assert.AreEqual(996, a[3]);
            Assert.AreEqual(995, a[4]);
        }
    }
}