using System.Collections.Concurrent;
using Alexa.NET.Request;
using Alexa.NET.Response;
using RoleShuffle.Application.Games.TheResistanceAvalon;

namespace RoleShuffle.Application.Games
{
    public abstract class BaseGame<TRound> : IGame where TRound : class
    {
        protected readonly ConcurrentDictionary<string, TRound> RunningRounds;

        protected BaseGame(string gameName, short gameNumber)
        {
            GameName = gameName;
            GameNumber = gameNumber;
            RunningRounds = new ConcurrentDictionary<string, TRound>();
        }

        public short GameNumber { get; }
        public string GameName { get; }

        public virtual bool IsPlaying(string userId)
        {
            return RunningRounds.ContainsKey(userId);
        }

        public abstract SkillResponse StartGameRequested(SkillRequest skillRequest);
        public abstract SkillResponse DistributeRoles(SkillRequest request);
        public abstract SkillResponse PerformNightPhase(SkillRequest request);
    }
}