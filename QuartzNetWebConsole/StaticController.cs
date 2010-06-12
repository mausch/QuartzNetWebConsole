using System.Web;
using MiniMVC;

namespace QuartzNetWebConsole {
    public class StaticController : Controller {
        public override IResult Execute(HttpContextBase context) {
            var resource = context.Request.QueryString["r"];
            var contentType = context.Request.QueryString["t"];
            if (contentType != null)
                context.Response.ContentType = contentType;
            var fullResourceName = string.Format("{0}.Resources.{1}", GetType().Assembly.FullName.Split(',')[0], resource);
            using (var resourceStream = GetType().Assembly.GetManifestResourceStream(fullResourceName)) {
                const int size = 32768;
                var buffer = new byte[size];
                var read = 0;
                while ((read = resourceStream.Read(buffer, 0, size)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, read);
                return new EmptyResult();
            }
        }
    }
}