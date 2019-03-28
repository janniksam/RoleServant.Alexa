using System.Text;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using RoleShuffle.Application.Extensions;
using RoleShuffle.Application.ResponseMessages;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.SecretHitler
{
    public class SecretHitlerGame : BaseGame<SecretHitlerRound>
    {
        private const short MinPlayers = 5;
        private const short MaxPlayers = 10;
        private readonly IMessages m_messages;
        
        public SecretHitlerGame(IMessages messages)
            : base("Secret Hitler", Constants.GameNumbers.SecretHitler)
        {
            m_messages = messages;
        }

        public override SkillResponse PerformNightPhase(SkillRequest request)
        {
            return ResponseBuilder.Tell($"In dem Spiel \"{GameName}\" gibt es keine Nachtphase. Versuche es stattdessen mit \"Starte die Rollenverteilung\".");
        }

        public override SkillResponse DistributeRoles(SkillRequest request)
        {
            if (!RunningRounds.TryGetValue(request.Context.System.User.UserId, out var round) || round == null)
            {
                return ResponseBuilder.Tell($"Für das Spiel {GameName} ist keine aktive Runde vorhanden.");
            }

            var builder = new StringBuilder();
            builder.AppendLine("<speak>");
            builder.AppendLine("    <p>");
            builder.AppendLine("        Die Rollenverteilung beginnt in");
            builder.AppendLine("        5<break time='0.3s' />");
            builder.AppendLine("        4<break time='0.3s' />");
            builder.AppendLine("        3<break time='0.3s' />");
            builder.AppendLine("        2<break time='0.3s' />");
            builder.AppendLine("        1<break time='0.8s' />");

            if (round.PlayerAmount == 5 ||round.PlayerAmount == 6)
            {
                builder.AppendLine("        Alle Spieler schließen bitte jetzt die Augen!");
                builder.AppendLine("        <break time='5s' />");
                builder.AppendLine("        Faschist und Hitler öffnen die Augen und geben sich gegenseitig zu erkennen in ");
                builder.AppendLine("        3<break time='0.3s' />");
                builder.AppendLine("        2<break time='0.3s' />");
                builder.AppendLine("        1<break time='0.3s' />");
                builder.AppendLine("        jetzt!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        OK! Hitler und Faschist schließen bitte ihre Augen in ");
                builder.AppendLine("        3<break time='0.3s' />");
                builder.AppendLine("        2<break time='0.3s' />");
                builder.AppendLine("        1<break time='0.3s' />");
                builder.AppendLine("        jetzt!");
                builder.AppendLine("        <break time='4s' />");
            }
            else
            {
                builder.AppendLine("        Alle Spieler strecken ihre Faust bitte nach vorne oder legen diese klar sichtbar vor sich auf den Tisch!");
                builder.AppendLine("        <break time='5s' />");
                builder.AppendLine("        Alle Spieler schließen bitte jetzt die Augen!");
                builder.AppendLine("        <break time='5s' />");
                builder.AppendLine("        Die Faschisten öffnen die Augen und Erkennen einander in ");
                builder.AppendLine("        3<break time='0.3s' />");
                builder.AppendLine("        2<break time='0.3s' />");
                builder.AppendLine("        1<break time='0.3s' />");
                builder.AppendLine("        jetzt!");
                builder.AppendLine("        <break time='7s' />");
                builder.AppendLine("        Hitler hebt jetzt bitte den Daumen, aber lässt die Augen zu.");
                builder.AppendLine("        <break time='2s' />");
                builder.AppendLine("        Die Faschisten identifizieren Hitler.");
                builder.AppendLine("        <break time='7s' />");
                builder.AppendLine("        Alle Spieler ziehen die Arme an ihren Körper und schließen ihre Augen.");
                builder.AppendLine("        <break time='7s' />");
            }

            builder.AppendLine("        Alle Spieler dürfen ihre Augen wieder weit öffnen und das Spiel beginnt.");
            builder.AppendLine("    </p>");
            builder.AppendLine("</speak>");
            
            return ResponseBuilder.Tell(new SsmlOutputSpeech {Ssml = builder.ToString()});
        }

        public override SkillResponse StartGameRequested(SkillRequest skillRequest)
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