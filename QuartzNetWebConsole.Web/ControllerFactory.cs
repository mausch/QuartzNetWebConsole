using System;
using System.Linq;
using System.Web;
using MiniMVC;
using QuartzNetWebConsole.Utils;
using Response = QuartzNetWebConsole.Utils.Response;
using Route = System.Collections.Generic.KeyValuePair<string, System.Action<System.Web.HttpContextBase>>;

namespace QuartzNetWebConsole {
    public class ControllerFactory : HttpHandlerFactory {
        public override IHttpHandler GetHandler(HttpContextBase context) {
            var lastUrlSegment = context.Request.Url.Segments.Last().Split('.')[0];
            return Routing.Routes.Where(k => k.Key.Equals(lastUrlSegment, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value)
                .Select(ToWebAction)
                .Select(h => new HttpHandlerWithReadOnlySession(h))
                .FirstOrDefault();
        }

        private static Action<HttpContextBase> ToWebAction(Func<RelativeUri, Response> handler) {
            return ctx => {
                var response = handler(new RelativeUri(ctx.Request.Url.PathAndQuery));
                EvaluateResponse(response, ctx.Response);
            };
        }

        private static void EvaluateResponse(Response response, HttpResponseBase httpResponse) {
            response.MatchAction(
                content: x => httpResponse.Raw(x.Content, x.ContentType),
                xdoc: x => httpResponse.XDocument(x.Content, x.ContentType),
                redirect: x => httpResponse.Redirect(x.Location));
        }

    }
}