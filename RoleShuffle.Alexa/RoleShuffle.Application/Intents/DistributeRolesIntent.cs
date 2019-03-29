using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.Games;
using RoleShuffle.Application.ResponseMessages;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Intents
{
    public class DistributeRolesIntent : IIntent
    {
        private readonly IEnumerable<IGame> m_availableGames;
        private readonly IMessages m_messages;

        public DistributeRolesIntent(
            IEnumerable<IGame> availableGames,
            IMessages messages)
        {
            m_availableGames = availableGames;
            m_messages = messages;
        }

        public bool IsResponseFor(string intent)
        {
            return intent == Constants.Intents.DistributeRoles;
        }

        public async Task<SkillResponse> GetResponse(SkillRequest skillRequest)
        {
            var usersGame =
                m_availableGames.FirstOrDefault(p => p.IsPlaying(skillRequest.Context.System.User.UserId));
            if (usersGame == null)
            {
                return ResponseBuilder.Tell(m_messages.ErrorNoOpenGame);
            }

            return usersGame.DistributeRoles(skillRequest);
        }
    }
}