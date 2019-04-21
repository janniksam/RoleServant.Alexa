using System;
using System.Collections.Generic;
using System.Linq;
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
        private const string ChooseDeckIdView = "ChooseDeckId";
        private const string ChooseDeckIdConfirmationView = "ChooseDeckIdConfirmation";
        private const string ChooseDeckIdReAskView = "ChooseDeckIdReAsk";

        private readonly IOneNightUltimateWerewolfRoleManager m_roleManager;

        public OneNightUltimateWerewolfGame(IOneNightUltimateWerewolfRoleManager roleManager)
            : base("OneNightUltimateWerewolf", Constants.GameNumbers.OneNightUltimateWerewolf)
        {
            m_roleManager = roleManager;
        }

        public override IEnumerable<string> GetRequiredSSMLViews()
        {
            return base.GetRequiredSSMLViews()
                .Concat(new[]
                {
                    ChooseDeckIdReAskView,
                    ChooseDeckIdConfirmationView,
                    ChooseDeckIdView,
                    NightPhaseView
                });
        }

        public override Task<SkillResponse> StartGameRequested(SkillRequest request)
        {
            var intentRequest = (IntentRequest)request.Request;
            var deckIdRaw = intentRequest.Intent.GetSlot(Constants.Slots.DeckId);
            if (!int.TryParse(deckIdRaw, out var deckId))
            {
                return AskForDeckId(request);
            }

            var roleSelection = m_roleManager.GetRoleSelection(deckId);
            if (roleSelection == null)
            {
                return ReAskForDeckId(request);
            }

            var confirmValue = intentRequest.Intent.GetSlot(Constants.Slots.ConfirmAction);
            if (confirmValue == null)
            {
                return AskForDeckConfimation(request, roleSelection);
            }

            if (!confirmValue.Equals(Constants.SlotYesNoResult.Yes, StringComparison.InvariantCultureIgnoreCase))
            {
                intentRequest.Intent.Slots[Constants.Slots.ConfirmAction].Value = null;
                intentRequest.Intent.Slots[Constants.Slots.DeckId].Value = null;
                return AskForDeckId(request, intentRequest.Intent);
            }

            var userId = request.Context.System.User.UserId;
            var newRound = new OneNightUltimateWerewolfRound(roleSelection);
            RunningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);

            return PerformDefaultStartGamePhaseWithNightPhaseContinuation(request);
        }

        private async Task<SkillResponse> AskForDeckId(SkillRequest request, Intent updatedIntent = null)
        {
            var ssml = await GetSSMLAsync(ChooseDeckIdView, request.Request.Locale).ConfigureAwait(false);
            return ResponseBuilder.DialogElicitSlot(
                new SsmlOutputSpeech
                {
                    Ssml = ssml
                },
                Constants.Slots.DeckId, 
                updatedIntent);
        }

        private async Task<SkillResponse> AskForDeckConfimation(SkillRequest request, RoleSelection roleSelection)
        {
            if (roleSelection == null)
            {
                throw new ArgumentNullException(nameof(roleSelection));
            }

            var ssml = await GetSSMLAsync(ChooseDeckIdConfirmationView, request.Request.Locale, roleSelection).ConfigureAwait(false);
            return ResponseBuilder.DialogElicitSlot(
                new SsmlOutputSpeech
                {
                    Ssml = ssml
                },
                Constants.Slots.ConfirmAction);
        }

        private async Task<SkillResponse> ReAskForDeckId(SkillRequest request)
        {
            var ssml = await GetSSMLAsync(ChooseDeckIdReAskView, request.Request.Locale).ConfigureAwait(false);
            return ResponseBuilder.DialogElicitSlot(
                new SsmlOutputSpeech
                {
                    Ssml = ssml
                },
                Constants.Slots.DeckId);
        }

        public override Task<SkillResponse> PerformNightPhase(SkillRequest request)
        {
            if (!RunningRounds.TryGetValue(request.Context.System.User.UserId, out var round) || round == null)
            {
                return NoActiveGameOpen(request);
            }

            return PerformDefaultNightPhase(request, round);
        }
    }
}