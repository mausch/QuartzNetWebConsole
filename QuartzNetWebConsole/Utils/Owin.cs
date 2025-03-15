using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzNetWebConsole.Utils {
    public static class Owin {
        public static string GetOwinRequestPath(this IDictionary<string, object> env) {
            return (string)env["owin.RequestPath"];
        }

        public static string GetOwinRequestQueryString(this IDictionary<string, object> env) {
            return (string)env["owin.RequestQueryString"];
        }

        public static RelativeUri GetOwinRelativeUri(this IDictionary<string, object> env) {
            return new RelativeUri(env.GetOwinRequestPath() + "?" + env.GetOwinRequestQueryString());
        }

        public static Stream GetOwinResponseBody(this IDictionary<string, object> env) {
            return (Stream) env["owin.ResponseBody"];
        }

        public static IDictionary<string, string[]> GetOwinResponseHeaders(this IDictionary<string, object> env) {
            return (IDictionary<string, string[]>) env["owin.ResponseHeaders"];
        }

        public static void SetOwinContentType(this IDictionary<string, object> env, string contentType, string charset) {
            if (string.IsNullOrEmpty(contentType))
                throw new ArgumentNullException("contentType");
            if (string.IsNullOrEmpty(charset))
                throw new ArgumentNullException("charset");
            env.GetOwinResponseHeaders()["Content-Type"] = new[] {contentType + ";charset=" + charset};
        }

        public static void SetOwinContentLength(this IDictionary<string, object> env, long length) {
            env.GetOwinResponseHeaders()["Content-Length"] = new[] {length.ToString()};
        }

        public static void SetOwinStatusCode(this IDictionary<string, object> env, int statusCode) {
            env["owin.ResponseStatusCode"] = statusCode;
        }

        private static readonly Encoding encoding = Encoding.UTF8;

        public static Setup.AppFunc EvaluateResponse(this Response response) {
            return env => response.Match(
                content: async x => {
                    env.SetOwinContentType(contentType: x.ContentType, charset: encoding.BodyName);
                    var content = Encoding.UTF8.GetBytes(x.Content);
                    env.SetOwinContentLength(content.Length);
                    await env.GetOwinResponseBody().WriteAsync(content, 0, content.Length);
                },
                xdoc: async x => {
                    var eval = new Response.ContentResponse(content: x.Content.ToString(), contentType: x.ContentType).EvaluateResponse();
                    await eval(env);
                },
                redirect: async x => {
                    env.SetOwinStatusCode(302);
                    env.GetOwinResponseHeaders()["Location"] = new[] { x.Location };
                    await Task.Yield();
                });
        }


    }
}