using System.Text;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.ResponseMessages;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.TheResistanceAvalon
{
    public class TheResistanceAvalonGame : BaseGame<TheResistanceAvalonRound>
    {
        private readonly IMessages m_messages;

        public TheResistanceAvalonGame(IMessages messages)
            : base("The Resistance Avalon", Constants.GameNumbers.TheResistanceAvalon)
        {
            m_messages = messages;
        }

        public override SkillResponse PerformNightPhase(SkillRequest request)
        {
            return ResponseBuilder.Tell($"In dem Spiel \"{GameName}\" gibt es keine Nachtphase. Versuche es stattdessen mit \"Starte die Rollenverteilung\".");
        }

        public override SkillResponse DistributeRoles(SkillRequest request)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<speak>");
            builder.AppendLine("    <p>");
            builder.AppendLine("        Die Rollenverteilung beginnt in");
            builder.AppendLine("        5<break time='0.3s' />");
            builder.AppendLine("        4<break time='0.3s' />");
            builder.AppendLine("        3<break time='0.3s' />");
            builder.AppendLine("        2<break time='0.3s' />");
            builder.AppendLine("        1<break time='0.8s' />");
            builder.AppendLine("        Alle Spieler schließen bitte jetzt die Augen!");
            builder.AppendLine("        <break time='5s' />");
            builder.AppendLine("        Alle Spieler strecken ihre Faust bitte nach vorne oder legen diese klar sichtbar vor sich auf den Tisch!");
            builder.AppendLine("        <break time='5s' />");

            builder.AppendLine("        Die Minions von Mordred öffnen die Augen und Erkennen einander in ");
            builder.AppendLine("        3<break time='0.3s' />");
            builder.AppendLine("        2<break time='0.3s' />");
            builder.AppendLine("        1<break time='0.3s' />");
            builder.AppendLine("        jetzt!");
            builder.AppendLine("        <break time='7s' />");
            builder.AppendLine("        Minions von Mordred, bitte schließt eure Augen.");
            builder.AppendLine("        <break time='3s' />");
            builder.AppendLine("        Die Minions von Mordred heben jetzt bitte den Daumen, lassen dabei aber die Augen zu.");
            builder.AppendLine("        <break time='5s' />");

            builder.AppendLine("        Merlin, bitte öffne die Augen und Identifiziere die Minions von Mordred in ");
            builder.AppendLine("        3<break time='0.3s' />");
            builder.AppendLine("        2<break time='0.3s' />");
            builder.AppendLine("        1<break time='0.3s' />");
            builder.AppendLine("        jetzt!");
            builder.AppendLine("        <break time='7s' />");
            builder.AppendLine("        Merlin, bitte schließe deine Augen wieder.");
            builder.AppendLine("        <break time='5s' />");

            builder.AppendLine("        Alle Spieler ziehen die Arme an ihren Körper und halten ihre Augen geschlossen.");
            builder.AppendLine("        <break time='5s' />");
            builder.AppendLine("        Alle Spieler dürfen ihre Augen wieder weit öffnen und die Runde beginnt.");
            builder.AppendLine("    </p>");
            builder.AppendLine("</speak>");

            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = builder.ToString() });
        }

        public override SkillResponse StartGameRequested(SkillRequest skillRequest)
        {
            var userId = skillRequest.Context.System.User.UserId;

            var newRound = new TheResistanceAvalonRound();
            RunningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);

            var response = ResponseBuilder.Tell(
                $"Die Runde {GameName} wurde gestartet. " +
                "Falls du jetzt mit der Rollenverteilung beginnen möchtest, kannst du jetzt \"Starte die Rollenverteilung\" sagen.");
            response.Response.ShouldEndSession = false;
            return response;
        }
    }
}