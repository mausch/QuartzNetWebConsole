using System;
using System.Linq;
using System.Web;
using QuartzNetWebConsole.Controllers;

namespace QuartzNetWebConsole {
    public class ControllerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return (IHttpHandler)Activator.CreateInstance(GetHandlerType(context.Request.RawUrl));
        }

        public Type GetHandlerType(string rawUrl)
        {
            var name = rawUrl.Split('/').Last().Split('?')[0].Split('.')[0];
            if (string.IsNullOrEmpty(name))
                return typeof(IndexController);

            string typeName = string.Format("QuartzNetWebConsole.Controllers.{0}Controller", name);
            var type = Type.GetType(typeName, true, true);
            if (type == null)
                throw new Exception(string.Format("Type '{0}' not found", type));
            return type;
        }

        public void ReleaseHandler(IHttpHandler handler) { }
    }
}