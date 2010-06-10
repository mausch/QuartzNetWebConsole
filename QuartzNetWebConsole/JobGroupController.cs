using System;
using System.Linq;
using System.Web;
using MiniMVC;
using Quartz;

namespace QuartzNetWebConsole {
    public class JobGroupController : Controller {
        private readonly IScheduler scheduler = Setup.Scheduler();

        public override IResult Execute(HttpContextBase context) {
            var group = context.Request.QueryString["group"];
            var jobNames = scheduler.GetJobNames(group);
            var jobs = jobNames.Select(j => scheduler.GetJobDetail(j, group));
            return new ViewResult(new {jobs, group}, ViewName);
        }
    }
}