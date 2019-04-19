using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RoleShuffle.Application.Abstractions.Services;

namespace RoleShuffle.Web.Services
{
    public class WarmUpService : IHostedService 
    {
        private readonly IApplicationWarmUp m_warmup;

        public WarmUpService(IApplicationWarmUp warmup)
        {
            m_warmup = warmup;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return m_warmup.WarmUp();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}