using System;
using System.Linq;
using System.Web;
using QuartzNetWebConsole.Controllers;

namespace QuartzNetWebConsole {
    public class ControllerFactory : IHttpHandlerFactory {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated) {
            var name = context.Request.RawUrl.Split('/').Last().Split('?')[0].Split('.')[0];
            if (name.ToLowerInvariant() == "scheduler")
                return new SchedulerController();
            string typeName = string.Format("QuartzNetWebConsole.Controllers.{0}Controller", name);
            var type = Type.GetType(typeName, true, true);
            if (type == null)
                throw new Exception(string.Format("Type '{0}' not found", type));
            return (IHttpHandler)Activator.CreateInstance(type);
        }

        public void ReleaseHandler(IHttpHandler handler) {}
    }
}