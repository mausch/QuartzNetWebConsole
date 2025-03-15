using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using QuartzNetWebConsole.Utils;

namespace QuartzNetWebConsole.Controllers {
    public class StaticController {
        private static readonly Assembly assembly = typeof(StaticController).Assembly;

        public static async Task<Response> Execute(RelativeUri url)
        {
            await Task.CompletedTask;
            var querystring = url.ParseQueryString();
            var resource = querystring["r"];
            resource = string.Format("{0}.Resources.{1}", assembly.FullName.Split(',')[0], resource);
            var content = ReadResource(resource);
            return new Response.ContentResponse(content: content, contentType: querystring["t"]);
        }

        public static string ReadResource(string name) {
            using (var stream = assembly.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }
    }
}