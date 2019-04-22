using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using RoleShuffle.Application.Extensions;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.SecretHitler
{
    public class SecretHitlerGame : BaseGame<SecretHitlerRound>
    {
        private const short MinPlayers = 5;
        private const short MaxPlayers = 10;

        public const string ChoosePlayerNumberBetweenView = "ChoosePlayerNumberBetween";
        private const string RoundStartedView = "RoundStarted";

        public SecretHitlerGame()
            : base("SecretHitler", Constants.GameNumbers.SecretHitler)
        {
        }

        public override IEnumerable<string> GetRequiredSSMLViews()
        {
            return base.GetRequiredSSMLViews()
                .Concat(new[]
                {
                    DistributeRolesView,
                    ChoosePlayerNumberBetweenView,
                    RoundStartedView
                });
        }

        public override Task<SkillResponse> DistributeRoles(SkillRequest request)
        {
            if (!RunningRounds.TryGetValue(request.Context.System.User.UserId, out var round) || round == null)
            {
                return NoActiveGameOpen(request);
            }

            return PerformDefaultDistributionPhase(request, round);
        }

        public override async Task<SkillResponse> StartGameRequested(SkillRequest skillRequest)
        {
            var request = (IntentRequest)skillRequest.Request;
            var playerAmountRaw = request.Intent.GetSlot(Constants.Slots.PlayerAmount);
            if (!short.TryParse(playerAmountRaw, out var playerAmount) || 
                playerAmount < MinPlayers || playerAmount > MaxPlayers)
            {
                return await ChoosePlayerNumber(skillRequest);
            }

            var newRound = new SecretHitlerRound(playerAmount);
            var userId = skillRequest.Context.System.User.UserId;
            RunningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);

            return await GameStarted(skillRequest, playerAmount);
        }

        private async Task<SkillResponse> GameStarted(SkillRequest skillRequest, short playerAmount)
        {
            var ssmlGameStarted = await GetSSMLAsync(RoundStartedView, skillRequest.Request.Locale, playerAmount);
            var response = ResponseBuilder.Tell(new SsmlOutputSpeech {Ssml = ssmlGameStarted});
            response.Response.ShouldEndSession = false;
            return response;
        }

        private async Task<SkillResponse> ChoosePlayerNumber(SkillRequest skillRequest)
        {
            var ssmlChoosePlayerNumber = await GetSSMLAsync(ChoosePlayerNumberBetweenView, skillRequest.Request.Locale,
                new[] {MinPlayers, MaxPlayers});
            return ResponseBuilder.DialogElicitSlot(
                new SsmlOutputSpeech
                {
                    Ssml = ssmlChoosePlayerNumber
                },
                Constants.Slots.PlayerAmount);
        }
    }
}