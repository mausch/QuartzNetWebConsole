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
            scheduler.AddGlobalJobListener(new GlobalJobListener());
            scheduler.AddGlobalTriggerListener(new GlobalTriggerListener());
            // construct job info
            var jobDetail = new JobDetail("myJob", null, typeof(HelloJob));
            // fire every hour
            var trigger = TriggerUtils.MakeHourlyTrigger();
            // start on the next even hour
            trigger.StartTimeUtc = TriggerUtils.GetEvenHourDate(DateTime.UtcNow);
            trigger.Name = "myTrigger";
            scheduler.ScheduleJob(jobDetail, trigger);
            Setup.Scheduler = () => scheduler;
        }

        protected void Session_Start(object sender, EventArgs e) {}

        protected void Application_BeginRequest(object sender, EventArgs e) {}

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {}

        protected void Application_Error(object sender, EventArgs e) {}

        protected void Session_End(object sender, EventArgs e) {}

        protected void Application_End(object sender, EventArgs e) {}
    }
}