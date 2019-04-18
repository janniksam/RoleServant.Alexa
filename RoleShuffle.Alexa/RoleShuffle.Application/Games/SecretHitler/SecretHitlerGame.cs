using System.Text;
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

            return PerformDefaultNightPhase(request, round);
        }

        public override async Task<SkillResponse> StartGameRequested(SkillRequest skillRequest)
        {
            var request = (IntentRequest)skillRequest.Request;
            var playerAmountRaw = request.Intent.GetSlot(Constants.Slots.PlayerAmount);
            if (!short.TryParse(playerAmountRaw, out var playerAmount) || playerAmount < MinPlayers || playerAmount > MaxPlayers)
            {
                return ResponseBuilder.DialogElicitSlot(
                    new PlainTextOutputSpeech
                    {
                        Text = $"Bitte wählen Sie für {GameName} eine Spielerzahl zwischen {MinPlayers} und {MaxPlayers} aus."
                    }, 
                    Constants.Slots.PlayerAmount);
            }

            var userId = skillRequest.Context.System.User.UserId;
            var newRound = new SecretHitlerRound(playerAmount);
            RunningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);
            
            var response = ResponseBuilder.Tell(
                $"{GameName} wurde mit einer Spieleranzahl von {playerAmount} gestartet. " +
                "Falls du jetzt mit der Rollenverteilung beginnen möchtest, kannst du jetzt \"Starte die Rollenverteilung\" sagen.");
            response.Response.ShouldEndSession = false;
            return response;
        }
    }
}