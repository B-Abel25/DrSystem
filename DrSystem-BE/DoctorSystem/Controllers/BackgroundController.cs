using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    public class BackgroundController : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                //await MyMethod();
                await Task.Delay(TimeSpan.FromMinutes(10), stopToken);
            }
        }

        public async Task<bool> ShouldStartTask()
        {
            Task.Delay(DateTime.Today.AddDays(1) - DateTime.Now);
            ExecuteAsync(new CancellationToken());

            return true;
        }
    }
}
