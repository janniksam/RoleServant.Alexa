using System;
using System.Threading;

namespace RoleShuffle.Application.Abstractions.Model
{
    public class GameStats : ICloneable
    {
        private long m_totalGamesCreated;
        private long m_totalNightPhases;
        private long m_totalDistributionPhases;

        public long TotalGamesCreated
        {
            get => m_totalGamesCreated;
            set => m_totalGamesCreated = value;
        }

        public long TotalDistributionPhases
        {
            get => m_totalDistributionPhases;
            set => m_totalDistributionPhases = value;
        }

        public long TotalNightPhases
        {
            get => m_totalNightPhases;
            set => m_totalNightPhases = value;
        }

        public void IncrementGameStarted()
        {
            Interlocked.Increment(ref m_totalGamesCreated);
        }

        public void IncrementNightPhases()
        {
            Interlocked.Increment(ref m_totalNightPhases);
        }

        public void IncrementDistributionPhases()
        {
            Interlocked.Increment(ref m_totalDistributionPhases);
        }

        public object Clone()
        {
            return new GameStats
            {
                TotalGamesCreated = TotalGamesCreated,
                TotalNightPhases = TotalNightPhases,
                TotalDistributionPhases = TotalDistributionPhases
            };
        }
    }
}