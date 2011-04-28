using System;
using System.Web;
using Quartz;
using Quartz.Impl;
using QuartzNetWebConsole;

namespace SampleApp {
    public class Global : HttpApplication {
        protected void Application_Start(object sender, EventArgs e) {
            // First, initialize Quartz.NET as usual. In this sample app I'll configure Quartz.NET by code.
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            // This tells the QuartzNetWebConsole what scheduler to use
            Setup.Scheduler = () => scheduler;

            // This adds an logger to the QuartzNetWebConsole. It's optional.
            var partialQuartzConsoleUrl = string.Format("http://{0}:{1}/quartz/", Context.Request.Url.Host, Context.Request.Url.Port);
            Setup.Logger = new MemoryLogger(1000, partialQuartzConsoleUrl);

            // I'll add some global listeners
            scheduler.AddGlobalJobListener(new GlobalJobListener());
            scheduler.AddGlobalTriggerListener(new GlobalTriggerListener());

            // A sample trigger and job
            var trigger = TriggerUtils.MakeSecondlyTrigger(6);
            trigger.StartTimeUtc = DateTime.UtcNow;
            trigger.Name = "myTrigger";
            scheduler.ScheduleJob(new JobDetail("myJob", null, typeof(HelloJob)), trigger);

            // A cron trigger and job
            var cron = new CronTrigger("myCronTrigger") {
                CronExpression = new CronExpression("0/10 * * * * ?"), // every 10 seconds
                JobName = "myJob",
            };
            scheduler.ScheduleJob(cron);

            // A dummy calendar
            scheduler.AddCalendar("myCalendar", new DummyCalendar {Description = "dummy calendar"}, false, false);
        }

        protected void Session_Start(object sender, EventArgs e) {}

        protected void Application_BeginRequest(object sender, EventArgs e) {}

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {}

        protected void Application_Error(object sender, EventArgs e) {}

        protected void Session_End(object sender, EventArgs e) {}

        protected void Application_End(object sender, EventArgs e) {}
    }
}