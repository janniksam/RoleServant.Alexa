using System.Threading;
using System.Threading.Tasks;
using RoleShuffle.Base.Aspects;

namespace RoleShuffle.Application.Abstractions.Services
{
    [LogMethodScope]
    public interface IConfigurationManager
    {
        Task LoadAsync(CancellationToken cancellationToken);

        Task WriteToDiskAsync(CancellationToken cancellationToken);
    }
}