using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace WebApi.HostedServices
{
    public class QueueService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: implement
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // TODO: implement
            return Task.CompletedTask;
        }
    }
}