using System;
using System.Linq;
using NUnit.Framework;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Tests {
    [TestFixture]
    public class ViewsTests {
        [Test]
        public void TrClassAlt() {
            var x = Views.Views.SchedulerCalendars(new[] {
                Helpers.KV("one", "1"),
                Helpers.KV("two", "2"),
                Helpers.KV("three", "3"),
                Helpers.KV("four", "4"),
            });
            Helpers.StripeTrs(x);
            //Console.WriteLine(x.ToString());
            var trs = x.Descendants("tr")
                .Skip(1)
                .WhereOdd();
            foreach (var tr in trs) {
                Console.WriteLine(tr.ToString());
                Assert.AreEqual("alt", tr.Attribute("class").Value);                
            }
        }
    }
}