using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Games.SecretHitler;

namespace RoleShuffle.Application.Tests.Games.SecretHitler
{
    [TestClass]
    public class OneNightUltimateWerewolfTests : GameTestBase
    {
        [TestInitialize]
        public void TestFixture()
        {
            var game = new SecretHitlerGame();
            Initialize(game);
        }
    }
}