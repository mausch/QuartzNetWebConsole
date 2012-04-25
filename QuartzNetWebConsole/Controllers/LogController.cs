using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MiniMVC;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class LogController : Controller {
        private readonly IQueryable<LogEntry> logsQ = Setup.Logger ?? Enumerable.Empty<LogEntry>().AsQueryable();

        public int DefaultPageSize { get; set; }

        public LogController() {
            DefaultPageSize = 25;
        }

        public override void Execute(HttpContextBase context) {
            var qs = context.Request.QueryString;
            var thisUrl = context.Request.Url.ToString().Split('?')[0];
            var pageSize = GetPageSize(qs);
            var pagination = new PaginationInfo {
                FirstItemIndex = GetStartIndex(qs),
                PageSize = pageSize,
                TotalItemCount = logsQ.Count(),
                PageUrl = "log.ashx?start=!0&max=" + pageSize,
            };
            var logs = logsQ.Skip(pagination.FirstItemIndex).Take(pagination.PageSize).ToList();
            var v = GetView(qs.AllKeys);
            var view = v.Value(logs, pagination, thisUrl);
            context.XDocument(view, contentType: v.Key);
        }

        public KeyValuePair<string, Func<IEnumerable<LogEntry>, PaginationInfo, string, XDocument>> GetView(IEnumerable<string> qs) {
            if (qs.Contains("rss"))
                return Helpers.KV("application/rss+xml", RSSView);
            return Helpers.KV((string)null, XHTMLView);
        }

        public static readonly Func<IEnumerable<LogEntry>, PaginationInfo, string, XDocument> XHTMLView =
            (entries, pagination, url) => Helpers.XHTML(Views.Views.Log(entries, pagination, url));

        public static readonly Func<IEnumerable<LogEntry>, PaginationInfo, string, XDocument> RSSView =
            (entries, pagination, url) => new XDocument(Views.Views.LogRSS(url, entries));

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