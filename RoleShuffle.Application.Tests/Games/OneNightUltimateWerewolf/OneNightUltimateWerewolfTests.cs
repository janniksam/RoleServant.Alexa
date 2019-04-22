using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Abstractions.Model;
using RoleShuffle.Application.Games;
using RoleShuffle.Application.Games.OneNightUltimateWerewolf;
using RoleShuffle.Application.RoleManager;
using RoleShuffle.Application.SSMLResponses;
using SSMLBuilder;
using SSMLVerifier;

namespace RoleShuffle.Application.Tests.Games.OneNightUltimateWerewolf
{
    [TestClass]
    public class OneNightUltimateWerewolfTests : GameTestBase
    {
        [TestInitialize]
        public void TestFixture()
        {
            var game = new OneNightUltimateWerewolfGame(new OneNightUltimateWerewolfRoleManager());
            Initialize(game);
        }

        [TestMethod]
        public async Task RoleSummaryReturnsValidSSML()
        {
            var ssml = await CommonResponseCreator.GetGameSpecificSSMLAsync(
                "OneNightUltimateWerewolf", 
                "ChooseDeckIdConfirmation", 
                "de-DE",
                new RoleSelection
                {
                    Drunk = 2,
                    Mason = 5,
                    Hunter = 12
                });

            var ssmlValidationErrors = Verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());

            ssml = await CommonResponseCreator.GetGameSpecificSSMLAsync(
                "OneNightUltimateWerewolf",
                "ChooseDeckIdConfirmation",
                "en-US",
                new RoleSelection
                {
                    Drunk = 2,
                    Mason = 2,
                    Hunter = 1,
                    Villager = 3,
                    Werewolf = 2
                });

            ssmlValidationErrors = Verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());

            ssml = await CommonResponseCreator.GetGameSpecificSSMLAsync(
                "OneNightUltimateWerewolf",
                "ChooseDeckIdConfirmation",
                "en-GB",
                new RoleSelection
                {
                    Drunk = 2,
                    Mason = 2,
                    Hunter = 1,
                    Villager = 3,
                    Werewolf = 2
                });

            ssmlValidationErrors = Verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        protected override object GetTestModelFor(string view)
        {
            switch (view)
            {
                case OneNightUltimateWerewolfGame.ChooseDeckIdConfirmationView:
                    return GetTestRound().RoleSelection;
                case BaseGame<OneNightUltimateWerewolfGame>.NightPhaseView:
                    return GetTestRound();
            }

            return null;
        }

        private static OneNightUltimateWerewolfRound GetTestRound()
        {
            return new OneNightUltimateWerewolfRound(new RoleSelection
            {
                Drunk = 1,
                Mason = 2,
                Doppelganger = 1,
                Seer = 1,
                Werewolf = 1,
                Minion = 1,
                Villager = 2,
                Hunter = 1,
                Insomniac = 1,
                Robber = 1,
                Tanner = 1,
                Troublemaker = 1
            });
        }
    }
}