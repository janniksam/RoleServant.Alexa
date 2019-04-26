using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RoleShuffle.Application.Abstractions.Services;

namespace RoleShuffle.Web.Services
{
    public class ConfigurationWorkerService : IHostedService 
    {
        private readonly IConfigurationWorker m_configurationWorker;

        public ConfigurationWorkerService(IConfigurationWorker configurationWorker)
        {
            m_configurationWorker = configurationWorker;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await m_configurationWorker.DoWork(cancellationToken).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}