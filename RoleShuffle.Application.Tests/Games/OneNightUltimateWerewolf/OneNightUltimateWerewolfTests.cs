using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Abstractions.Model;
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
            var ssml = await CommonResponseCreator.GetSSMLAsync(typeof(OneNightUltimateWerewolfRound),
                typeof(OneNightUltimateWerewolfRound).Namespace + ".SSMLViews", "ChooseDeckIdConfirmation", "de-DE",
                new RoleSelection
                {
                    Drunk = 2,
                    Mason = 5,
                    Hunter = 12
                });

            var ssmlValidationErrors = Verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }
    }
}