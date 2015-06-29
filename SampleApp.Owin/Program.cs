using System;
using Microsoft.Owin.Hosting;
using Owin;
using Quartz;
using Quartz.Impl;

namespace SampleApp.Owin {
    class Program {
        static void Start(IAppBuilder app) {

            // First, initialize Quartz.NET as usual. In this sample app I'll configure Quartz.NET by code.
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            // I'll add some global listeners
            //scheduler.ListenerManager.AddJobListener(new GlobalJobListener());
            //scheduler.ListenerManager.AddTriggerListener(new GlobalTriggerListener());

            // A sample trigger and job
            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger")
                .WithSchedule(DailyTimeIntervalScheduleBuilder.Create()
                    .WithIntervalInSeconds(6))
                .StartNow()
                .Build();
            var job = new JobDetailImpl("myJob", null, typeof(HelloJob));
            scheduler.ScheduleJob(job, trigger);

            // A cron trigger and job
            var cron = TriggerBuilder.Create()
                .WithIdentity("myCronTrigger")
                .ForJob(job.Key)
                .WithCronSchedule("0/10 * * * * ?") // every 10 seconds
                .Build();

            scheduler.ScheduleJob(cron);

            // A dummy calendar
            //scheduler.AddCalendar("myCalendar", new DummyCalendar { Description = "dummy calendar" }, false, false);

            app.Use(QuartzNetWebConsole.Setup.Owin(() => scheduler));
        }

        private static void Main(string[] args) {
            using (WebApp.Start("http://localhost:12345", Start))
                Console.ReadLine();
        }
    }
}