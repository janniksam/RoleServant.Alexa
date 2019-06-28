using System;
using System.Threading;
using System.Threading.Tasks;
using RoleShuffle.Base.Aspects;

namespace RoleShuffle.Application.Abstractions.Services
{
    [LogMethodScope]
    public interface IConfigurationManager
    {
        DateTime GetLastBackupDate();

        Task LoadAsync(CancellationToken cancellationToken);

        Task WriteToDiskAsync(CancellationToken cancellationToken);
    }
}