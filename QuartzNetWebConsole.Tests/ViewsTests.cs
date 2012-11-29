using System;
using System.Linq;
using MiniMVC;
using NUnit.Framework;
using Quartz;
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

        [Test]
        public void TriggerTable() {
            var trigger = new DummyTrigger {
                Key = new TriggerKey("myCronTrigger", "DEFAULT"),
                JobKey = new JobKey("someJob", "DEFAULT"),
            };
            var triggers = new[] {
                new TriggerWithState(trigger, TriggerState.Normal),
            };
            var x = Views.Views.TriggerTable(triggers, "/", highlight: "DEFAULT.myCronTrigger");
            var tr = x.Descendants().First(e => {
                var id = e.Attribute("id");
                if (id == null)
                    return false;
                return id.Value == "DEFAULT.myCronTrigger";
            });
            Assert.AreEqual("highlight", tr.Attribute("class").Value);
            //var html = x.MakeHTMLCompatible();
            //Console.WriteLine(html);
        }
    }
}