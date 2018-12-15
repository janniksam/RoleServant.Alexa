using System;
using System.Collections.Concurrent;
using System.Text;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using RoleShuffle.Application.Abstractions.Model;
using RoleShuffle.Application.Abstractions.RoleManager;
using RoleShuffle.Application.Extensions;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.OneNightUltimateWerewolf
{
    public class OneNightUltimateWerewolfGame : IGame
    {
        private readonly IOneNightUltimateWerewolfRoleManager m_roleManager;
        private readonly ConcurrentDictionary<string, OneNightUltimateWerewolfRound> m_runningRounds;
        private const string IdIpa = "<phoneme alphabet=\"ipa\" ph=\"ˈaiˈdiː\">ID</phoneme>";

        public OneNightUltimateWerewolfGame(IOneNightUltimateWerewolfRoleManager roleManager)
        {
            m_roleManager = roleManager;
            m_runningRounds = new ConcurrentDictionary<string, OneNightUltimateWerewolfRound>();
        }

        public SkillResponse StartGameRequested(SkillRequest skillRequest)
        {
            var request = (IntentRequest)skillRequest.Request;
            var deckIdRaw = request.Intent.GetSlot(Constants.Slots.DeckId);
            if (!int.TryParse(deckIdRaw, out var deckId))
            {
                return AskForDeckId();
            }

            var roleSelection = m_roleManager.GetRoleSelection(deckId);
            if (roleSelection == null)
            {
                return ReAskForDeckId();
            }

            var confirmValue = request.Intent.GetSlot(Constants.Slots.ConfirmAction);
            if (confirmValue == null)
            {
                return AskForDeckConfimation(roleSelection);
            }
            if(!confirmValue.Equals(Constants.SlotResult.Yes, StringComparison.InvariantCultureIgnoreCase))
            {
                request.Intent.Slots[Constants.Slots.ConfirmAction].Value = null;
                request.Intent.Slots[Constants.Slots.DeckId].Value = null;
                return AskForDeckId(request.Intent);
            }

            var userId = skillRequest.Context.System.User.UserId;
            var newRound = new OneNightUltimateWerewolfRound(roleSelection);
            m_runningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);

            var response = ResponseBuilder.Tell(
                $"Die Runde {GameName} wurde gestartet. " +
                "Falls du jetzt mit der Nacht Phase beginnen möchtest, kannst du jetzt \"Starte die Nacht Phase\" sagen.");
            response.Response.ShouldEndSession = false;
            return response;
        }

        private SkillResponse AskForDeckId(Intent updatedIntent = null)
        {
            var outputText = "<speak>";
            outputText += $"Bitte wählen Sie für {GameName} eine Deck-{IdIpa} aus, die sie sich vorher generiert haben lassen.";
            outputText += "</speak>";
            return ResponseBuilder.DialogElicitSlot(
                new SsmlOutputSpeech
                {
                    Ssml = outputText
                },
                Constants.Slots.DeckId, 
                updatedIntent);
        }

        private SkillResponse AskForDeckConfimation(RoleSelection roleSelection)
        {
            if (roleSelection == null)
            {
                throw new ArgumentNullException(nameof(roleSelection));
            }

            var outputText = "<speak>";
            outputText += "<p>Das ausgewählte Deck enthält folgende Karten:</p>";
            outputText += $"<p>{roleSelection.DeckSummary}.</p>";
            outputText += "<p>Soll das Spiel mit diesem Deck gestartet werden?</p>";
            outputText += "</speak>";
            return ResponseBuilder.DialogElicitSlot(
                new SsmlOutputSpeech
                {
                    Ssml = outputText
                },
                Constants.Slots.ConfirmAction);
        }

        private SkillResponse ReAskForDeckId()
        {
            var outputText = "<speak>";
            outputText += $"<p>Die Deck-{IdIpa} konnte nicht gefunden werden.</p>";
            outputText +=
                $"<p>Bitte wählen Sie für {GameName} eine Deck-{IdIpa} aus, die sie sich vorher generiert haben lassen.</p>";
            outputText += "</speak>";
            return ResponseBuilder.DialogElicitSlot(
                new SsmlOutputSpeech
                {
                    Ssml = outputText
                },
                Constants.Slots.DeckId);
        }

        public SkillResponse DistributeRoles(SkillRequest request)
        {
            return ResponseBuilder.Tell($"In dem Spiel {GameName} gibt es keine Rollenverteilung");
        }

        public SkillResponse PerformNightPhase(SkillRequest request)
        {
            if (!m_runningRounds.TryGetValue(request.Context.System.User.UserId, out var round) || round == null)
            {
                return ResponseBuilder.Tell($"Für das Spiel {GameName} ist keine aktive Runde vorhanden.");
            }

            var builder = new StringBuilder();
            builder.AppendLine("<speak>");
            builder.AppendLine("    <p>");
            builder.AppendLine("        Die Nachtphase beginnt in");
            builder.AppendLine("        5<break time='0.3s' />");
            builder.AppendLine("        4<break time='0.3s' />");
            builder.AppendLine("        3<break time='0.3s' />");
            builder.AppendLine("        2<break time='0.3s' />");
            builder.AppendLine("        1<break time='0.8s' />");
            builder.AppendLine("        Alle Spieler schließen bitte jetzt die Augen!");
            builder.AppendLine("        <break time='5s' />");

            if (round.RoleSelection.Doppelganger > 0)
            {
                builder.AppendLine("        Dorfgängerin, bitte öffne deine Augen!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        Dorfgängerin, bitte schließe deine Augen wieder.");
                builder.AppendLine("        <break time='3s' />");
            }

            if (round.RoleSelection.Werewolf > 0)
            {
                builder.AppendLine("        Werwölfe, bitte öffnet eure Augen!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        Werwölfe, bitte schließt eure Augen wieder.");
                builder.AppendLine("        <break time='3s' />");
            }

            if (round.RoleSelection.Minion > 0)
            {
                builder.AppendLine("        Günstling, bitte öffne deine Augen!");
                builder.AppendLine("        <break time='1.5s' />");
                builder.AppendLine("        Werwölfe, bitte hebt mit Augen zu eure Daumen, damit der Günstling euch identifizieren kann!");
                builder.AppendLine("        <break time='7s' />");
                builder.AppendLine("        Günstling, bitte schließe deine Augen.");
                builder.AppendLine("        Werwölfe, bitte nehmt jetzt eure Hände wieder in eine natürliche Position zurück.");
                builder.AppendLine("        <break time='3s' />");
            }

            if (round.RoleSelection.Mason > 0)
            {
                builder.AppendLine("        Freimaurer, bitte öffnet die Augen!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        Freimaurer, bitte schließt eure Augen.");
                builder.AppendLine("        <break time='3s' />");
            }

            if (round.RoleSelection.Seer > 0)
            {
                builder.AppendLine("        Seher, bitte öffne deine Augen!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        Seher, bitte schließe deine Augen.");
                builder.AppendLine("        <break time='3s' />");
            }

            if (round.RoleSelection.Robber > 0)
            {
                builder.AppendLine("        Räuber, bitte öffne deine Augen!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        Räuber, bitte schließe deine Augen.");
                builder.AppendLine("        <break time='3s' />");
            }

            if (round.RoleSelection.Troublemaker > 0)
            {
                builder.AppendLine("        Unruhestifterin, bitte öffne deine Augen!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        Unruhestifterin, bitte schließe deine Augen.");
                builder.AppendLine("        <break time='3s' />");
            }

            if (round.RoleSelection.Drunk > 0)
            {
                builder.AppendLine("        Betrunkener, bitte öffne deine Augen!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        Betrunkener, bitte schließe deine Augen.");
                builder.AppendLine("        <break time='3s' />");
            }

            if (round.RoleSelection.Insomniac > 0)
            {
                builder.AppendLine("        Schlaflose, bitte öffne deine Augen!");
                builder.AppendLine("        <break time='10s' />");
                builder.AppendLine("        Schlaflose, bitte schließe deine Augen.");
                builder.AppendLine("        <break time='3s' />");
            }

            builder.AppendLine("        Alle Spieler dürfen ihre Augen wieder weit öffnen und die Tagesphase beginnt.");
            builder.AppendLine("    </p>");
            builder.AppendLine("</speak>");

            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = builder.ToString() });
        }

        public bool IsPlaying(string userId)
        {
            return m_runningRounds.ContainsKey(userId);
        }

        public short GameNumber => Constants.GameNumbers.OneNightUltimateWerewolf;

        public string GameName => "Vollmondnacht Werwölfe";
    }
}