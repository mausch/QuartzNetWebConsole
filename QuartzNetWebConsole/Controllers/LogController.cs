using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniMVC;

namespace QuartzNetWebConsole.Controllers {
    public class LogController : Controller {
        private readonly IEnumerable<LogEntry> logs = Setup.Logger ?? Enumerable.Empty<LogEntry>();

        public override IResult Execute(HttpContextBase context) {
            string contentType = null;
            if (context.Request.QueryString.AllKeys.Contains("rss")) {
                ViewName = "QuartzNetWebConsole.Resources.Rss.html";
                contentType = "application/rss+xml";
            }
            var thisUrl = context.Request.Url.ToString().Split('?')[0];
            return new ViewResult(new {logs, thisUrl}, ViewName) {
                ContentType = contentType,
            };
        }
    }
}