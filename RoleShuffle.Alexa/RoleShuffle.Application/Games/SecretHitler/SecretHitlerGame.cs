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
        private const string ChooseNumberBetweenView = "ChooseNumberBetween";
        private const string RoundStartedView = "RoundStarted";

        public SecretHitlerGame()
            : base("Secret Hitler", Constants.GameNumbers.SecretHitler)
        {
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
            if (!short.TryParse(playerAmountRaw, out var playerAmount) || playerAmount < MinPlayers || playerAmount > MaxPlayers)
            {
                var ssmlChoosePlayerNumber = await GetSSMLAsync(ChooseNumberBetweenView, skillRequest.Request.Locale, new {MinPlayers, MaxPlayers});                
                return ResponseBuilder.DialogElicitSlot(
                    new SsmlOutputSpeech
                    {
                        Ssml = ssmlChoosePlayerNumber
                    }, 
                    Constants.Slots.PlayerAmount);
            }

            var userId = skillRequest.Context.System.User.UserId;
            var newRound = new SecretHitlerRound(playerAmount);
            RunningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);

            var ssmlGameStarted = await GetSSMLAsync(RoundStartedView, skillRequest.Request.Locale, playerAmount);
            var response = ResponseBuilder.Tell(new SsmlOutputSpeech {Ssml = ssmlGameStarted});
            response.Response.ShouldEndSession = false;
            return response;
        }
    }
}