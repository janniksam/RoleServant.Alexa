using System.Collections.Generic;
using System.Threading.Tasks;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Abstractions.Services;
using RoleShuffle.Application.SSMLResponses;

namespace RoleShuffle.Application.Services
{
    public class ApplicationWarmUp : IApplicationWarmUp
    {
        public async Task WarmUp()
        {
            await CommonResponseCreator.GetSSMLAsync(MessageKeys.ChooseGame, "de-DE", new List<IGame>());
            await CommonResponseCreator.GetSSMLAsync(MessageKeys.ChooseGame, "en-US", new List<IGame>());
        }
    }
}