using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Games.TheResistanceAvalon;

namespace RoleShuffle.Application.Tests.Games.TheResistanceAvalon
{
    [TestClass]
    public class TheResistanceAvalonGameTests : GameTestBase
    {
        [TestInitialize]
        public void TestFixture()
        {
            var game = new TheResistanceAvalonGame();
            Initialize(game);
        }
    }
}