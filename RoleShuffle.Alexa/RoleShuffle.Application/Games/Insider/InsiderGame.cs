using System.Collections.Concurrent;
using System.Text;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.ResponseMessages;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.Insider
{
    public class InsiderGame : IGame
    {
        private readonly IMessages m_messages;
        private readonly ConcurrentDictionary<string, InsiderRound> m_runningRounds;
        
        public InsiderGame(IMessages messages)
        {
            m_messages = messages;
            m_runningRounds = new ConcurrentDictionary<string, InsiderRound>();
        }

        public short GameNumber => Constants.GameNumbers.Insider;

        public string GameName => "Insider";

        public SkillResponse PerformNightPhase(SkillRequest request)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<speak>");
            builder.AppendLine("    <p>");
            builder.AppendLine("        Die Nachtphase beginnt in");
            builder.AppendLine("        5<break time='0.3s' />");
            builder.AppendLine("        4<break time='0.3s' />");
            builder.AppendLine("        3<break time='0.3s' />");
            builder.AppendLine("        2<break time='0.3s' />");
            builder.AppendLine("        1<break time='0.8s' />");
            builder.AppendLine("        Alle Spieler bis auf den Spielleiter schließen bitte jetzt die Augen!");
            builder.AppendLine("        <break time='5s' />");

            builder.AppendLine("        Spielleiter, bitte drehe jetzt die Karte um merke dir den Begriff.");
            builder.AppendLine("        <break time='8s' />");
            builder.AppendLine("        Spielleiter, bitte schließe deine Augen.");
            builder.AppendLine("        <break time='3s' />");

            builder.AppendLine("        Insider, öffne deine Augen und schaue dir den Begriff an.");
            builder.AppendLine("        <break time='8s' />");
            builder.AppendLine("        Insider, bitte schließe deine Augen.");
            builder.AppendLine("        <break time='3s' />");

            builder.AppendLine("        Alle Spieler dürfen ihre Augen wieder weit öffnen und die Runde beginnt.");
            builder.AppendLine("    </p>");
            builder.AppendLine("</speak>");

            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = builder.ToString() });
        }

        public bool IsPlaying(string userId)
        {
            return m_runningRounds.ContainsKey(userId);
        }

        public SkillResponse DistributeRoles(SkillRequest request)
        {
            return ResponseBuilder.Tell($"In dem Spiel {GameName} gibt es keine Rollenverteilung");
        }

        public SkillResponse StartGameRequested(SkillRequest skillRequest)
        {
            var userId = skillRequest.Context.System.User.UserId;

            var newRound = new InsiderRound();
            m_runningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);
            
            var response = ResponseBuilder.Tell(
                $"Die Runde {GameName} wurde gestartet. " +
                "Falls du jetzt mit der Nacht Phase beginnen möchtest, kannst du jetzt \"Starte die Nacht Phase\" sagen.");
            response.Response.ShouldEndSession = false;
            return response;
        }
    }
}