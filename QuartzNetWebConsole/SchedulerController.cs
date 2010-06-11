using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using MiniMVC;
using Quartz;

namespace QuartzNetWebConsole {
    public class SchedulerController : Controller {
        private readonly IScheduler scheduler = Setup.Scheduler();
        private static readonly MethodInfo[] methods = typeof (IScheduler).GetMethods();

        public override IResult Execute(HttpContextBase context) {
            var qs = context.Request.QueryString;
            var methodName = qs["method"].ToLowerInvariant();
            var redirect = qs["next"] ?? "index.ashx";
            var parameterNames = qs.AllKeys
                .Except(new[] {"method", "next"})
                .Select(a => a.ToLowerInvariant())
                .ToArray();
            var method = methods
                .Where(n => n.Name.ToLowerInvariant() == methodName)
                .Single(m => m.GetParameters()
                                 .Select(p => p.Name.ToLowerInvariant())
                                 .ToSet()
                                 .SetEquals(parameterNames));
            var parameters = method.GetParameters()
                .Select(p => Convert(qs[p.Name], p.ParameterType))
                .ToArray();
            method.Invoke(scheduler, parameters);
            return new RedirectResult(redirect);
        }

        public object Convert(string s, Type t) {
            var converter = TypeDescriptor.GetConverter(t);
            if (converter != null && converter.CanConvertFrom(typeof (string)))
                return converter.ConvertFromInvariantString(s);
            throw new Exception(string.Format("Can't convert to type '{0}'", t));
        }
    }
}