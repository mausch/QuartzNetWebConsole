using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using MiniMVC;
using QuartzNetWebConsole.Models;

namespace QuartzNetWebConsole.Controllers {
    public class LogController : Controller {
        private readonly IQueryable<LogEntry> logsQ = Setup.Logger ?? Enumerable.Empty<LogEntry>().AsQueryable();

        public int DefaultPageSize { get; set; }

        public LogController() {
            DefaultPageSize = 25;
        }

        public override IResult Execute(HttpContextBase context) {
            string contentType = null;
            var qs = context.Request.QueryString;
            if (qs.AllKeys.Contains("rss")) {
                ViewName = "QuartzNetWebConsole.Resources.Rss.html";
                contentType = "application/rss+xml";
            }
            var thisUrl = context.Request.Url.ToString().Split('?')[0];
            var pageSize = GetPageSize(qs);
            var pagination = new PaginationInfo {
                FirstItemIndex = GetStartIndex(qs),
                PageSize = pageSize,
                TotalItemCount = logsQ.Count(),
                PageUrl = "log.ashx?start=!0&max=" + pageSize,
            };
            var logs = logsQ.Skip(pagination.FirstItemIndex).Take(pagination.PageSize).ToList();
            return new ViewResult(new {logs, pagination, thisUrl}, ViewName) {
                ContentType = contentType,
            };
        }

        public int GetPageSize(NameValueCollection nv) {
            try {
                return int.Parse(nv["max"]);
            } catch {
                return DefaultPageSize;
            }
        }

        public int GetStartIndex(NameValueCollection nv) {
            try {
                return int.Parse(nv["start"]);
            } catch {
                return 0;
            }
        }
    }
}