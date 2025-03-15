using System;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using QuartzNetWebConsole.Utils;
using QuartzNetWebConsole.Views;

namespace QuartzNetWebConsole.Controllers {
    public class JobGroupController {
        public static async Task<Response> Execute(RelativeUri uri, Func<ISchedulerWrapper> getScheduler) {
            var scheduler = getScheduler();
            var querystring = QueryStringParser.ParseQueryString(uri.Query);

            var group = querystring["group"];
            var jobNames = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            var runningJobs = await scheduler.GetCurrentlyExecutingJobs();
            var jobs = await jobNames.Traverse(async j => {
                var job = await scheduler.GetJobDetail(j);
                // TODO apparently interruptible is not a thing any more? 
                //var interruptible = typeof (IInterruptableJob).IsAssignableFrom(job.JobType);
                var jobContext = runningJobs.FirstOrDefault(r => r.JobDetail.Key.ToString() == job.Key.ToString());
                return new JobWithContext(job, jobContext, false);
            });
            var paused = await scheduler.IsJobGroupPaused(group);
            var highlight = querystring["highlight"];
            var view = Views.Views.JobGroup(group, paused, highlight, uri.PathAndQuery, jobs);
            return new Response.XDocumentResponse(Helpers.XHTML(view));
        }
    }
}