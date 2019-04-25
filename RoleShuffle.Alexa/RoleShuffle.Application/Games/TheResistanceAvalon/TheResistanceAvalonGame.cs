using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using RoleShuffle.Application.Extensions;
using RoleShuffle.Base;

namespace RoleShuffle.Application.Games.TheResistanceAvalon
{
    public class TheResistanceAvalonGame : BaseGame<TheResistanceAvalonRound>
    {
        private const string SpecialCharPercivalPromptView = "SpecialCharPercivalPrompt";
        private const string SpecialCharMorganaPromptView = "SpecialCharMorganaPrompt";
        private const string SpecialCharMordredPromptView = "SpecialCharMordredPrompt";
        private const string SpecialCharOberonPromptView = "SpecialCharOberonPrompt";

        public TheResistanceAvalonGame()
            : base("TheResistanceAvalon", Constants.GameNumbers.TheResistanceAvalon)
        {
        }

        public override IEnumerable<string> GetRequiredSSMLViews()
        {
            return base.GetRequiredSSMLViews()
                .Concat(new[]
                {
                    DistributeRolesView,
                    SpecialCharPercivalPromptView,
                    SpecialCharMorganaPromptView,
                    SpecialCharMordredPromptView,
                    SpecialCharOberonPromptView
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

        public override async Task<SkillResponse> StartGameRequested(SkillRequest request)
        {
            var percivalResponse = await AskForCharacter(request, "AskForPercival", SpecialCharPercivalPromptView).ConfigureAwait(false);
            if (percivalResponse != null)
            {
                return percivalResponse;
            }

            var morganaResponse = await AskForCharacter(request, "AskForMorgana", SpecialCharMorganaPromptView).ConfigureAwait(false);
            if (morganaResponse != null)
            {
                return morganaResponse;
            }

            var mordredResponse = await AskForCharacter(request, "AskForMordred", SpecialCharMordredPromptView).ConfigureAwait(false);
            if (mordredResponse != null)
            {
                return mordredResponse;
            }

            var oberonResponse = await AskForCharacter(request, "AskForOberon", SpecialCharOberonPromptView).ConfigureAwait(false);
            if (oberonResponse != null)
            {
                return oberonResponse;
            }

            var witPercival = request.Session.Attributes["AskForPercival"].ToString() == Constants.SlotYesNoResult.Yes;
            var withMorgana = request.Session.Attributes["AskForMorgana"].ToString() == Constants.SlotYesNoResult.Yes;
            var withMordred = request.Session.Attributes["AskForMordred"].ToString() == Constants.SlotYesNoResult.Yes;
            var withOberon = request.Session.Attributes["AskForOberon"].ToString() == Constants.SlotYesNoResult.Yes;

            var userId = request.Context.System.User.UserId;
            var newRound = new TheResistanceAvalonRound
            {
                Morgana = withMorgana,
                Percival = witPercival,
                Mordred = withMordred,
                Oberon = withOberon,
                CreationLocale =  request.Request.Locale
            };
            RunningRounds.AddOrUpdate(userId, newRound, (k, v) => newRound);

            return await PerformDefaultStartGamePhaseWithDistributionPhaseContinuation(request).ConfigureAwait(false);
        }

        private async Task<SkillResponse> AskForCharacter(SkillRequest request, string sessionKey, string characterPrompt)
        {
            if(request.Session.Attributes == null)
            {
                request.Session.Attributes = new Dictionary<string, object>();
            }
            else if (request.Session.Attributes.ContainsKey(sessionKey))
            {
                return null;
            }

            var intentRequest = (IntentRequest)request.Request;
            var slotValue = intentRequest.Intent.GetSlot(Constants.Slots.ConfirmAction);
            if (!string.IsNullOrEmpty(slotValue) && IsYesOrNo(slotValue))
            {
                request.Session.Attributes.Add(sessionKey, slotValue);

                intentRequest.Intent.Slots[Constants.Slots.ConfirmAction].ConfirmationStatus = "None";
                intentRequest.Intent.Slots[Constants.Slots.ConfirmAction].Value = null;
                return null;
            }

            var ssml = await GetSSMLAsync(characterPrompt, request.Request.Locale)
                .ConfigureAwait(false);

            return ResponseBuilder.DialogElicitSlot(
                new SsmlOutputSpeech
                {
                    Ssml = ssml
                },
                Constants.Slots.ConfirmAction,
                request.Session,
                intentRequest.Intent);
        }

        private static bool IsYesOrNo(string slotValue)
        {
            return slotValue.Equals(Constants.SlotYesNoResult.Yes, StringComparison.InvariantCultureIgnoreCase) ||
                   slotValue.Equals(Constants.SlotYesNoResult.No, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}