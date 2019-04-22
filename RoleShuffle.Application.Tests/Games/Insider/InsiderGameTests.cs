using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Games.Insider;

namespace RoleShuffle.Application.Tests.Games.Insider
{
    [TestClass]
    public class InsiderGameTests : GameTestBase
    {
        [TestInitialize]
        public void TestFixture()
        {
            var game = new InsiderGame();
            Initialize(game);
        }

        protected override object GetTestModelFor(string view)
        {
            return null;
        }
    }
}