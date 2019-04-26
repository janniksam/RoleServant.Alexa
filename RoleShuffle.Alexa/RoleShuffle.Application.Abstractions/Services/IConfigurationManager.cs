using System.Threading;
using System.Threading.Tasks;

namespace RoleShuffle.Application.Abstractions.Services
{
    public interface IConfigurationManager
    {
        Task LoadAsync(CancellationToken cancellationToken);

        Task WriteToDiskAsync(CancellationToken cancellationToken);
    }
}