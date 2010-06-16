using System;
using System.Web;
using Quartz;
using Quartz.Impl;
using QuartzNetWebConsole;

namespace SampleApp {
    public class Global : HttpApplication {
        protected void Application_Start(object sender, EventArgs e) {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();
            Setup.Scheduler = () => scheduler;
            var partialQuartzConsoleUrl = string.Format("http://{0}:{1}/quartz/", Context.Request.Url.Host, Context.Request.Url.Port);
            Setup.SetLogger(new MemoryLogger(10, partialQuartzConsoleUrl));
            scheduler.AddGlobalJobListener(new GlobalJobListener());
            scheduler.AddGlobalTriggerListener(new GlobalTriggerListener());

            var trigger = TriggerUtils.MakeSecondlyTrigger(6);
            trigger.StartTimeUtc = DateTime.UtcNow;
            trigger.Name = "myTrigger";
            scheduler.ScheduleJob(new JobDetail("myJob", null, typeof(HelloJob)), trigger);

            var cron = new CronTrigger("myCronTrigger") {
                CronExpression = new CronExpression("0/10 * * * * ?"), // every 10 seconds
                JobName = "myJob",
            };
            scheduler.ScheduleJob(cron);
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