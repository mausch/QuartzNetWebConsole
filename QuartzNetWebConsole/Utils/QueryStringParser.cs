using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace QuartzNetWebConsole.Utils {
    public static class QueryStringParser {
        public static NameValueCollection ParseQueryString(this Uri uri) {
            if (string.IsNullOrEmpty(uri.Query) || uri.Query[0] != '?')
                return new NameValueCollection();
            var query = uri.Query.Substring(1).Split('&').Select(x => x.Split('=').Select(WebUtility.UrlDecode).ToArray());
            var r = new NameValueCollection();
            foreach (var kv in query) {
                var value = kv.Length < 2 ? "" : kv[1];
                r.Add(kv[0], value);
            }
            return r;
        }
    }
}