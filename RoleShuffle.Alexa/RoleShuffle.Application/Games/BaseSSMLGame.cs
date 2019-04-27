using System.Collections.Generic;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.SSMLResponses;

namespace RoleShuffle.Application.Games
{
    public abstract class BaseSSMLGame<TRound> : BaseGame<TRound>, IGame where TRound : IGameRound
    {
        protected BaseSSMLGame(string gameId, short gameNumber) : base(gameId, gameNumber)
        {
        }

        public abstract Task<SkillResponse> StartGameRequested(SkillRequest skillRequest);

        public virtual Task<SkillResponse> DistributeRoles(SkillRequest request)
        {
            return NoDistributionPhase(request);
        }

        public virtual Task<SkillResponse> PerformNightPhase(SkillRequest request)
        {
            return NoNightPhase(request);
        }

        public Task<string> GetLocalizedGamename(string locale)
        {
            return CommonResponseCreator.GetGameSpecificSSMLAsync(GameId, GameNamePartialView, locale);
        }

        public virtual IEnumerable<string> GetRequiredSSMLViews()
        {
            return new[] { GameNamePartialView };
        }

        protected Task<string> GetSSMLAsync(string action, string locale, object model = null)
        {
            return CommonResponseCreator.GetGameSpecificSSMLAsync(GameId, action, locale, model);
        }

        protected async Task<SkillResponse> PerformDefaultNightPhase(SkillRequest request)
        {
            var userId = request.Context.System.User.UserId;
            if (!RunningRounds.TryGetValue(userId, out var round) || round == null)
            {
                return await NoActiveGameOpen(request).ConfigureAwait(false);
            }

            round.UpdateLastUsed();
            NightPhaseStarted();

            var resultSSML = await GetSSMLAsync(NightPhaseView, request.Request.Locale, round).ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }

        protected async Task<SkillResponse> PerformDefaultDistributionPhase(SkillRequest request)
        {
            var userId = request.Context.System.User.UserId;
            if (!RunningRounds.TryGetValue(userId, out var round) || round == null)
            {
                return await NoActiveGameOpen(request).ConfigureAwait(false);
            }

            round.UpdateLastUsed();
            DistributionPhaseStarted();
            
            var resultSSML = await GetSSMLAsync(DistributeRolesView, request.Request.Locale, round)
                .ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }

        protected async Task<SkillResponse> PerformDefaultStartGamePhaseWithNightPhaseContinuation(SkillRequest request)
        {
            var gameName = await GetLocalizedGamename(request.Request.Locale).ConfigureAwait(false);
            var resultSSML =
                await CommonResponseCreator
                    .GetSSMLAsync(MessageKeys.GameStartContinueWithNightPhase, request.Request.Locale, gameName)
                    .ConfigureAwait(false);
            var response = ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
            response.Response.ShouldEndSession = false;
            return response;
        }

        protected async Task<SkillResponse> PerformDefaultStartGamePhaseWithDistributionPhaseContinuation(
            SkillRequest request)
        {
            var gameName = await GetLocalizedGamename(request.Request.Locale).ConfigureAwait(false);
            var resultSSML =
                await CommonResponseCreator
                    .GetSSMLAsync(MessageKeys.GameStartContinueWithDistributionPhase, request.Request.Locale, gameName)
                    .ConfigureAwait(false);
            var response = ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
            response.Response.ShouldEndSession = false;
            return response;
        }

        protected async Task<SkillResponse> NoActiveGameOpen(SkillRequest request)
        {
            var gameName = await GetLocalizedGamename(request.Request.Locale).ConfigureAwait(false);
            var resultSSML =
                await CommonResponseCreator
                    .GetSSMLAsync(MessageKeys.GameNoDistributionPhase, request.Request.Locale, gameName)
                    .ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }

        private async Task<SkillResponse> NoDistributionPhase(SkillRequest request)
        {
            var gameName = await GetLocalizedGamename(request.Request.Locale).ConfigureAwait(false);
            var resultSSML =
                await CommonResponseCreator
                    .GetSSMLAsync(MessageKeys.GameNoDistributionPhase, request.Request.Locale, gameName)
                    .ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }

        private async Task<SkillResponse> NoNightPhase(SkillRequest request)
        {
            var gameName = await GetLocalizedGamename(request.Request.Locale).ConfigureAwait(false);
            var resultSSML =
                await CommonResponseCreator.GetSSMLAsync(MessageKeys.GameNoNightPhase, request.Request.Locale, gameName)
                    .ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }
    }
}