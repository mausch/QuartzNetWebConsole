using System;
using System.Xml.Linq;

namespace QuartzNetWebConsole.Utils {
    public abstract class Response {
        private Response() { }

        public abstract T Match<T>(Func<ContentResponse, T> content, Func<XDocumentResponse, T> xdoc, Func<RedirectResponse, T> redirect);

        public void MatchAction(Action<ContentResponse> content, Action<XDocumentResponse> xdoc, Action<RedirectResponse> redirect) {
            Match<object>(x => {
                content(x);
                return null;
            }, x => {
                xdoc(x);
                return null;
            }, x => {
                redirect(x);
                return null;
            });
        }

        public sealed class ContentResponse : Response {
            public readonly string Content;
            public readonly string ContentType;

            public ContentResponse(string content, string contentType) {
                Content = content;
                ContentType = contentType;
            }

            public override T Match<T>(Func<ContentResponse, T> content, Func<XDocumentResponse, T> xdoc, Func<RedirectResponse, T> redirect) {
                return content(this);
            }
        }

        public sealed class XDocumentResponse : Response {
            public readonly XDocument Content;
            public readonly string ContentType;

            public XDocumentResponse(XDocument content, string contentType) {
                Content = content;
                ContentType = contentType;
            }

            public XDocumentResponse(XDocument content): this(content, "text/html") {}

            public override T Match<T>(Func<ContentResponse, T> content, Func<XDocumentResponse, T> xdoc, Func<RedirectResponse, T> redirect) {
                return xdoc(this);
            }
        }

        public sealed class RedirectResponse : Response {
            public readonly string Location;

            public RedirectResponse(string location) {
                Location = location;
            }

            public override T Match<T>(Func<ContentResponse, T> content, Func<XDocumentResponse, T> xdoc, Func<RedirectResponse, T> redirect) {
                return redirect(this);
            }
        }
    }
}
