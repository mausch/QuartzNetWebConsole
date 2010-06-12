using System.Web;
using MiniMVC;

namespace QuartzNetWebConsole {
    public class StaticController : Controller {
        public override IResult Execute(HttpContextBase context) {
            var resource = context.Request.QueryString["r"];
            resource = string.Format("{0}.Resources.{1}", GetType().Assembly.FullName.Split(',')[0], resource);
            return new ViewResult(null, resource) {
                ContentType = context.Request.QueryString["t"],
            };
        }
    }
}