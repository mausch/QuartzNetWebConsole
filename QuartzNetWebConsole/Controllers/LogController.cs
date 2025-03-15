using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class LogController {
        private static IQueryable<LogEntry> logsQ {
            get {
                return Setup.Logger ?? Enumerable.Empty<LogEntry>().AsQueryable();
            }
        }

        public static int DefaultPageSize { get; set; }

        static LogController() {
            DefaultPageSize = 25;
        }

        public static async Task<Response> Execute(RelativeUri url)
        {
            await Task.CompletedTask;
            var thisUrl = url.PathAndQuery.Split('?')[0];
            var qs = url.ParseQueryString();
            var pageSize = GetPageSize(qs);
            var pagination = new PaginationInfo(
                firstItemIndex: GetStartIndex(qs),
                pageSize: pageSize,
                totalItemCount: logsQ.Count(),
                pageUrl: "log.ashx?start=!0&max=" + pageSize);
            var logs = logsQ.Skip(pagination.FirstItemIndex).Take(pagination.PageSize).ToList();
            var v = GetView(qs.AllKeys);
            var view = v.Value(logs, pagination, thisUrl);
            return new Response.XDocumentResponse(view, v.Key);
        }

        public static KeyValuePair<string, Func<IEnumerable<LogEntry>, PaginationInfo, string, XDocument>> GetView(IEnumerable<string> qs) {
            if (qs.Contains("rss"))
                return Helpers.KV("application/rss+xml", RSSView);
            return Helpers.KV("text/html", XHTMLView);
        }

        public static readonly Func<IEnumerable<LogEntry>, PaginationInfo, string, XDocument> XHTMLView =
            (entries, pagination, url) => Helpers.XHTML(Views.Views.Log(entries, pagination, url));

        public static readonly Func<IEnumerable<LogEntry>, PaginationInfo, string, XDocument> RSSView =
            (entries, pagination, url) => new XDocument(Views.Views.LogRSS(url, entries));

        public static int GetPageSize(NameValueCollection nv) {
            try {
                return int.Parse(nv["max"]);
            } catch {
                return DefaultPageSize;
            }
        }

        public static int GetStartIndex(NameValueCollection nv) {
            try {
                return int.Parse(nv["start"]);
            } catch {
                return 0;
            }
        }
    }
}