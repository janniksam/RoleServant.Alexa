using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Abstractions.Model;

namespace RoleShuffle.Application.Games
{
    public abstract class BaseGame<TRound> where TRound : IGameRound
    {
        public const string NightPhaseView = "NightPhase";
        public const string DistributeRolesView = "DistributeRoles";

        protected const string GameNamePartialView = "GameNamePartial";
        protected readonly ConcurrentDictionary<string, TRound> RunningRounds;

        private GameStats m_stats = new GameStats();
        
        protected BaseGame(string gameId, short gameNumber)
        {
            GameId = gameId;
            GameNumber = gameNumber;
            RunningRounds = new ConcurrentDictionary<string, TRound>();
        }

        public string GameId { get; }

        public short GameNumber { get; }

        public virtual void StopPlaying(SkillRequest request)
        {
            var userId = request?.Context?.System?.User?.UserId;
            if (userId == null)
            {
                return;
            }

            RunningRounds.TryRemove(userId, out _);
        }

        public IEnumerable<IGameRound> GetOpenGames()
        {
            var openGames = new List<IGameRound>();
            var keys = RunningRounds.Keys.ToList();

            foreach (var key in keys)
            {
                if (RunningRounds.TryGetValue(key, out var value))
                {
                    openGames.Add(value);
                }
            }

            return openGames;
        }

        public GameStats GetCurrentStats()
        {
            return (GameStats)m_stats.Clone();
        }

        public void InitFromConfiguration(SavedGame savedGame)
        {
            m_stats = (GameStats) savedGame.GameStats.Clone();

            foreach (var round in savedGame.GameRounds)
            {
                if (!(round is TRound concreteRound))
                {
                    return;
                }

                RunningRounds.TryAdd(concreteRound.UserId, concreteRound);
            }
        }

        protected void CreateRound(TRound newRound)
        {
            RunningRounds.AddOrUpdate(newRound.UserId, newRound, (k, v) => newRound);
            m_stats.IncrementGameStarted();
        }

        protected void NightPhaseStarted()
        {
            m_stats.IncrementNightPhases();
        }

        protected void DistributionPhaseStarted()
        {
            m_stats.IncrementDistributionPhases();
        }

        public virtual bool IsPlaying(string userId)
        {
            return RunningRounds.ContainsKey(userId);
        }
    }
}