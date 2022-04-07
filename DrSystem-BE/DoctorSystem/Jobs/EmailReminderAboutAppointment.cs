using Quartz;
using System.Threading.Tasks;
using System;

namespace DoctorSystem.Jobs
{
    public class EmailReminderAboutAppointment : IJob
    {
        public EmailReminderAboutAppointment()
        {
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("emailReminder at {0}", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
