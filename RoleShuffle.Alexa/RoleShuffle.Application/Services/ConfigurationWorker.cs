using System;
using System.Threading;
using System.Threading.Tasks;
using RoleShuffle.Application.Abstractions.Services;

namespace RoleShuffle.Application.Services
{
    public class ConfigurationWorker : IConfigurationWorker
    {
        private readonly IConfigurationManager m_configurationManager;

        public ConfigurationWorker(IConfigurationManager configurationManager)
        {
            m_configurationManager = configurationManager;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            await m_configurationManager.LoadAsync(cancellationToken).ConfigureAwait(false);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(20), cancellationToken).ConfigureAwait(false);
                    await m_configurationManager.WriteToDiskAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}