using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace QuartzNetWebConsole.Utils {
    public static class QueryStringParser {
        public static NameValueCollection ParseQueryString(string q) {
            if (string.IsNullOrEmpty(q))
                return new NameValueCollection();
            var query = q.Split('&').Select(x => x.Split('=').Select(WebUtility.UrlDecode).ToArray());
            var r = new NameValueCollection();
            foreach (var kv in query) {
                var value = kv.Length < 2 ? "" : kv[1];
                r.Add(kv[0], value);
            }
            return r;
        }

        public static NameValueCollection ParseQueryString(this RelativeUri uri) {
            return ParseQueryString(uri.Query);
        }
    }
}