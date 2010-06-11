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
            var runningJobs = scheduler.GetCurrentlyExecutingJobs().Cast<JobExecutionContext>();
            var jobs = jobNames.Select(j => {
                var job = scheduler.GetJobDetail(j, group);
                var jobContext = runningJobs.FirstOrDefault(r => r.JobDetail.FullName == job.FullName);
                return new { job, jobContext, };
            });
            var paused = scheduler.IsJobGroupPaused(group);
            var thisUrl = context.Request.RawUrl;
            return new ViewResult(new {jobs, group, paused, thisUrl}, ViewName);
        }
    }
}