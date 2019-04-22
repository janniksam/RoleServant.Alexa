using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Games;
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

        protected override object GetTestModelFor(string view)
        {
            switch (view)
            {
                case BaseGame<TheResistanceAvalonGame>.DistributeRolesView:
                    return GetTestRound();
            }

            return null;
        }

        private TheResistanceAvalonRound GetTestRound()
        {
            return new TheResistanceAvalonRound
            {
                Morgana = true,
                Mordred = true,
                Oberon = true,
                Percival = true,
            };
        }
    }
}