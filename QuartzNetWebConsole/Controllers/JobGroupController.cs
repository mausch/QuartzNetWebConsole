using System.Linq;
using System.Web;
using System.Xml.Linq;
using MiniMVC;
using Quartz;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class JobGroupController : Controller {
        private readonly IScheduler scheduler = Setup.Scheduler();

        public override IResult Execute(HttpContextBase context) {
            var group = context.Request.QueryString["group"];
            var jobNames = scheduler.GetJobNames(group);
            var runningJobs = scheduler.GetCurrentlyExecutingJobs().Cast<JobExecutionContext>();
            var jobs = jobNames.Select(j => {
                var job = scheduler.GetJobDetail(j, group);
                var interruptible = typeof (IInterruptableJob).IsAssignableFrom(job.JobType);
                var jobContext = runningJobs.FirstOrDefault(r => r.JobDetail.FullName == job.FullName);
                return new JobWithContext(job, jobContext, interruptible);
            });
            var paused = scheduler.IsJobGroupPaused(group);
            var thisUrl = context.Request.RawUrl;
            var highlight = context.Request.QueryString["highlight"];
            var view = Views.Views.JobGroup(group, paused, highlight, thisUrl, jobs);
            return new XDocResult(Helpers.XHTML(view));
        }
    }
}