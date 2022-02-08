using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace WebApi.HostedServices
{
    //на самом деле не используется, т.к. AddMassTransitHostedService запускает это автоматически (или я не понял смысла QueueService)
    public class QueueService : IHostedService
    {
        private readonly IBusControl _bus;

        public QueueService(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _bus.StopAsync(cancellationToken);
        }
    }
}