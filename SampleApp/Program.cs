using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace SampleApp
{
    public class Program
    {
        public static IScheduler Scheduler { get; private set; } 
            
        public static async Task Main(string[] args)
        {
            // First, initialize Quartz.NET as usual. In this sample app I'll configure Quartz.NET by code.
            var schedulerFactory = new StdSchedulerFactory();
            Scheduler = await schedulerFactory.GetScheduler();
            await Scheduler.Start();

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
            await Scheduler.ScheduleJob(job, trigger);

            // A cron trigger and job
            var cron = TriggerBuilder.Create()
                .WithIdentity("myCronTrigger")
                .ForJob(job.Key)
                .WithCronSchedule("0/10 * * * * ?") // every 10 seconds
                .Build();

            await Scheduler.ScheduleJob(cron);

            // A dummy calendar
            //scheduler.AddCalendar("myCalendar", new DummyCalendar { Description = "dummy calendar" }, false, false);

            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}