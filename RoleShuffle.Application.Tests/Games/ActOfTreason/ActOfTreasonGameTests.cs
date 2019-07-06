using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Games.ActOfTreason;

namespace RoleShuffle.Application.Tests.Games.Insider
{
    [TestClass]
    public class ActOfTreasonGameTests : GameTestBase
    {
        [TestInitialize]
        public void TestFixture()
        {
            var game = new ActOfTreasonGame();
            Initialize(game);
        }

        protected override object GetTestModelFor(string view)
        {
            return null;
        }
    }
}