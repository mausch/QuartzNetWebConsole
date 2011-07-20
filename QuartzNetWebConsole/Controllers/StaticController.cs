using System.IO;
using System.Reflection;
using System.Web;
using MiniMVC;

namespace QuartzNetWebConsole.Controllers {
    public class StaticController : Controller {
        public override IResult Execute(HttpContextBase context) {
            var resource = context.Request.QueryString["r"];
            resource = string.Format("{0}.Resources.{1}", GetType().Assembly.FullName.Split(',')[0], resource);
            var content = ReadResource(resource);
            return new RawResult(content) {
                ContentType = context.Request.QueryString["t"],
            };
        }

        public string ReadResource(string name) {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }            
        }
    }
}