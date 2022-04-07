using DoctorSystem.Jobs;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace DoctorSystem.Services
{
    public class QuartzService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<MyJob> _myJobs;

        public IScheduler Scheduler { get; set; }
        
        public QuartzService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<MyJob> myJob
            )
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _myJobs = myJob;
        }

        public async Task StartAsync (CancellationToken cancelToken)
        {
            Console.WriteLine("Start at async {0}", DateTime.Now);
            Scheduler.JobFactory = _jobFactory;
            foreach (var myJob in _myJobs)
            {
                var job = CreateJob(myJob);
                var trigger = CreateTrigger(myJob);
                await Scheduler.ScheduleJob(job, trigger, cancelToken);
            }
            await Scheduler.Start(cancelToken);
        }

        public async Task StopAsync(CancellationToken cancelToken)
        {
            Console.WriteLine("Stop at async {0}", DateTime.Now);
            await Scheduler?.Shutdown(cancelToken);
        }
        private static IJobDetail CreateJob(MyJob myJob)
        {
            var type = myJob.Type;
            return JobBuilder.Create(type).WithIdentity(type.FullName).WithDescription(type.Name).Build();
        }

        private static ITrigger CreateTrigger(MyJob myJob)
        {
            var type = myJob.Type;
            return TriggerBuilder.Create().WithIdentity($"{myJob.Type.FullName}").WithCronSchedule(myJob.Expression).WithDescription(myJob.Expression).Build();
        }
    }
}
