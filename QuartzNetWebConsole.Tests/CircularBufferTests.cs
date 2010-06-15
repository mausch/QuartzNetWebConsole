using System;
using System.Linq;
using NUnit.Framework;
using QuartzNetWebConsole.Utils;

namespace QuartzNetWebConsole.Tests {
    [TestFixture]
    public class CircularBufferTests {
        [Test]
        public void tt() {
            var b = new CircularBuffer<int>(5);
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