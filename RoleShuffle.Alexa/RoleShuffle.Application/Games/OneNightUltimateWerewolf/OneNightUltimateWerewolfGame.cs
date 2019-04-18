using System;
using System.Threading.Tasks;
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
    public class OneNightUltimateWerewolfGame : BaseGame<OneNightUltimateWerewolfRound>
    {
        private readonly IOneNightUltimateWerewolfRoleManager m_roleManager;
        private const string IdIpa = "<phoneme alphabet=\"ipa\" ph=\"ˈaiˈdiː\">ID</phoneme>";

        public OneNightUltimateWerewolfGame(IOneNightUltimateWerewolfRoleManager roleManager)
            : base("Vollmondnacht Werwölfe", Constants.GameNumbers.OneNightUltimateWerewolf)
        {
            m_roleManager = roleManager;
        }

        public override async Task<SkillResponse> StartGameRequested(SkillRequest skillRequest)
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
            RunningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);

            return await PerformDefaultStartGamePhaseWithNightPhaseContinuation(skillRequest).ConfigureAwait(false);
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

        public override async Task<SkillResponse> PerformNightPhase(SkillRequest request)
        {
            if (!RunningRounds.TryGetValue(request.Context.System.User.UserId, out var round) || round == null)
            {
                return ResponseBuilder.Tell($"Für das Spiel {GameName} ist keine aktive Runde vorhanden.");
            }

            return await PerformDefaultNightPhase(request, round).ConfigureAwait(false);
        }
    }
}