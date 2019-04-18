using System.Collections.Concurrent;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.SSMLResponses;

namespace RoleShuffle.Application.Games
{
    public abstract class BaseGame<TRound> : IGame where TRound : class
    {
        private const string ActionNightPhase = "NightPhase";
        private const string ActionDistributeRoles = "DistributeRoles";

        protected readonly ConcurrentDictionary<string, TRound> RunningRounds;

        protected BaseGame(string gameName, short gameNumber)
        {
            GameName = gameName;
            GameNumber = gameNumber;
            RunningRounds = new ConcurrentDictionary<string, TRound>();
        }
        
        public short GameNumber { get; }

        public string GameName { get; }

        protected Task<string> GetSSMLAsync(string action, string locale, object model = null)
        {
            var type = GetType();
            var resourceNamespace = $"{type.Namespace}.SSMLViews";
            return CommonResponseCreator.GetSSMLAsync(type, resourceNamespace, action, locale, model);
        }

        public virtual bool IsPlaying(string userId)
        {
            return RunningRounds.ContainsKey(userId);
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

        protected async Task<SkillResponse> PerformDefaultNightPhase(SkillRequest request, object model = null)
        {
            var resultSSML = await GetSSMLAsync(ActionNightPhase, request.Request.Locale, model).ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }

        protected async Task<SkillResponse> PerformDefaultDistributionPhase(SkillRequest request, object model = null)
        {
            var resultSSML = await GetSSMLAsync(ActionDistributeRoles, request.Request.Locale, model)
                .ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }


        protected async Task<SkillResponse> PerformDefaultStartGamePhaseWithNightPhaseContinuation(SkillRequest request)
        {
            var resultSSML =
                await CommonResponseCreator
                    .GetSSMLAsync(MessageKeys.GameStartContinueWithNightPhase, request.Request.Locale, GameName)
                    .ConfigureAwait(false);
            var response = ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
            response.Response.ShouldEndSession = false;
            return response;
        }

        protected async Task<SkillResponse> PerformDefaultStartGamePhaseWithDistributionPhaseContinuation(SkillRequest request)
        {
            var resultSSML =
                await CommonResponseCreator
                    .GetSSMLAsync(MessageKeys.GameStartContinueWithDistributionPhase, request.Request.Locale, GameName)
                    .ConfigureAwait(false);
            var response = ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
            response.Response.ShouldEndSession = false;
            return response;
        }

        protected async Task<SkillResponse> NoActiveGameOpen(SkillRequest request)
        {
            var resultSSML =
                await CommonResponseCreator.GetSSMLAsync(MessageKeys.GameNoDistributionPhase, request.Request.Locale, GameName).ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }

        private async Task<SkillResponse> NoDistributionPhase(SkillRequest request)
        {
            var resultSSML =
                await CommonResponseCreator.GetSSMLAsync(MessageKeys.GameNoDistributionPhase, request.Request.Locale, GameName).ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }

        private async Task<SkillResponse> NoNightPhase(SkillRequest request)
        {
            var resultSSML =
                await CommonResponseCreator.GetSSMLAsync(MessageKeys.GameNoNightPhase, request.Request.Locale, GameName).ConfigureAwait(false);
            return ResponseBuilder.Tell(new SsmlOutputSpeech { Ssml = resultSSML });
        }
    }
}