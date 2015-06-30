using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using QuartzNetWebConsole.Utils;

namespace QuartzNetWebConsole.Controllers {
    public class SchedulerController {
        private static readonly MethodInfo[] methods = typeof(SchedulerWrapper).GetMethods();

        public class MethodParameters {
            public readonly MethodInfo method;
            public readonly string redirect;
            public readonly IEnumerable<object> parameters;

            public MethodParameters(MethodInfo method, string redirect, IEnumerable<object> parameters) {
                this.method = method;
                this.redirect = redirect;
                this.parameters = parameters;
            }
        }

        public static Func<MethodInfo, bool> MatchParameters(IEnumerable<string> parameterNames) {
            return m => m.GetParameters()
                        .Select(p => p.Name.ToLowerInvariant())
                        .ToSet()
                        .SetEquals(parameterNames);
        }

        public static MethodParameters GetMethodParameters(NameValueCollection qs) {
            var methodName = qs["method"].ToLowerInvariant();
            var redirect = qs["next"] ?? "index.ashx";
            var parameterNames = qs.AllKeys
                .Except(new[] { "method", "next" })
                .Select(a => a.ToLowerInvariant())
                .ToArray();
            var method = methods
                .Where(n => n.Name.ToLowerInvariant() == methodName)
                .Where(MatchParameters(parameterNames))
                .FirstOrDefault();

            if (method == null)
                throw new Exception("Method not found: " + methodName);

            var parameters = method.GetParameters()
                .Select(p => Convert(qs[p.Name], p.ParameterType))
                .ToArray();
            return new MethodParameters(method, redirect, parameters);
        }

        public static Response Execute(RelativeUri url, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var p = GetMethodParameters(url.ParseQueryString());
            p.method.Invoke(scheduler, p.parameters.ToArray());
            return new Response.RedirectResponse(p.redirect);
        }

        public static object Convert(string s, Type t) {
            var converter = TypeDescriptor.GetConverter(t);
            if (converter != null && converter.CanConvertFrom(typeof (string)))
                return converter.ConvertFromInvariantString(s);
            throw new Exception(string.Format("Can't convert to type '{0}'", t));
        }
    }
}