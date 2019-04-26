using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using RoleShuffle.Application.Extensions;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.SecretHitler
{
    public class SecretHitlerGame : BaseSSMLGame<SecretHitlerRound>
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
            return PerformDefaultDistributionPhase(request);
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

            var userId = skillRequest.Context.System.User.UserId;
            var newRound = new SecretHitlerRound
            {
                UserId = userId,
                CreationLocale = request.Locale,
                PlayerAmount = playerAmount
            };

            CreateRound(newRound);

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