using System.Linq;
using Xunit;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Tests {
    public class PaginationInfoTests {
        [Fact]
        public void tt() {
            var p = new PaginationInfo(
                firstItemIndex: 350,
                pageSize: 25,
                pageSlide:  2,
                totalItemCount: 398,
                pageUrl: "");
            Assert.Equal(16, p.LastPage);
            Assert.Equal(15, p.CurrentPage);
            Assert.True(p.HasNextPage);
            var pages = p.Pages.ToArray();
            Assert.Equal(5, pages.Length);
            Assert.Equal(12, pages[0]);
            Assert.Equal(13, pages[1]);
            Assert.Equal(14, pages[2]);
            Assert.Equal(15, pages[3]);
            Assert.Equal(16, pages[4]);
        }
    }
}