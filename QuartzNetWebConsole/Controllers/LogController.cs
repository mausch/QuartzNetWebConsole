using System;
using System.Web;
using MiniMVC;

namespace QuartzNetWebConsole.Controllers {
    public class LogController : Controller {
        private readonly ILogger logger = Setup.Logger;

        public override IResult Execute(HttpContextBase context) {
            if (logger == null)
                return new RawResult("No logger defined");
            return new ViewResult(logger, ViewName);
        }
    }
}