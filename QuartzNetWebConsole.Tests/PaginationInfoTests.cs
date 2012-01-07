using System.Linq;
using NUnit.Framework;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Tests {
    public static class PaginationInfoTests {
        public static void tt() {
            var p = new PaginationInfo {
                FirstItemIndex = 350,
                PageSize = 25,
                PageSlide = 2,
                TotalItemCount = 398,
            };
            Assert.AreEqual(16, p.LastPage);
            Assert.AreEqual(15, p.CurrentPage);
            Assert.IsTrue(p.HasNextPage);
            var pages = p.Pages.ToArray();
            Assert.AreEqual(5, pages.Length);
            Assert.AreEqual(12, pages[0]);
            Assert.AreEqual(13, pages[1]);
            Assert.AreEqual(14, pages[2]);
            Assert.AreEqual(15, pages[3]);
            Assert.AreEqual(16, pages[4]);
        }
    }
}