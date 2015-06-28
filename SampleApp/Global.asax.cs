using System;
using System.Web;
using Quartz;
using Quartz.Impl;
using QuartzNetWebConsole;

namespace SampleApp {
    public class Global : HttpApplication {

        static readonly HttpRequest InitialRequest;

        static Global() {
            // Workaround to fix error "Request is not available in this context" with IIS7 or IIS Express
            // http://sammyageil.com/post/Request-is-not-available-in-this-context-exception-in-Globalasaxs-Application_Start-IIS-7-Integrated-mode
            InitialRequest = HttpContext.Current.Request;
        }

        protected void Application_Start(object sender, EventArgs e) {
            // First, initialize Quartz.NET as usual. In this sample app I'll configure Quartz.NET by code.
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            // This tells the QuartzNetWebConsole what scheduler to use
            Setup.Scheduler = () => scheduler;

            // This adds an logger to the QuartzNetWebConsole. It's optional.
            var partialQuartzConsoleUrl = string.Format("http://{0}:{1}/quartz/", InitialRequest.Url.Host, InitialRequest.Url.Port);
            Setup.Logger = new MemoryLogger(1000, partialQuartzConsoleUrl);

            // I'll add some global listeners
            scheduler.ListenerManager.AddJobListener(new GlobalJobListener());
            scheduler.ListenerManager.AddTriggerListener(new GlobalTriggerListener());

            // A sample trigger and job
            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger")
                .WithSchedule(DailyTimeIntervalScheduleBuilder.Create()
                    .WithIntervalInSeconds(6))
                .StartNow()
                .Build();
            var job = new JobDetailImpl("myJob", null, typeof (HelloJob));
            scheduler.ScheduleJob(job, trigger);

            // A cron trigger and job
            var cron = TriggerBuilder.Create()
                .WithIdentity("myCronTrigger")
                .ForJob(job.Key)
                .WithCronSchedule("0/10 * * * * ?") // every 10 seconds
                .Build();

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