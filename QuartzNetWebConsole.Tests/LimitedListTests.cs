using System;
using System.Linq;
using Xunit;
using QuartzNetWebConsole.Utils;

namespace QuartzNetWebConsole.Tests {
    public class LimitedListTests {
        [Fact]
        public void tt() {
            var b = new LimitedList<int>(5);
            foreach (var i in Enumerable.Range(0, 1000))
                b.Add(i);
            var a = b.ToArray();
            Assert.Equal(5, a.Length);
            Assert.Equal(999, a[0]);
            Assert.Equal(998, a[1]);
            Assert.Equal(997, a[2]);
            Assert.Equal(996, a[3]);
            Assert.Equal(995, a[4]);
        }
    }
}