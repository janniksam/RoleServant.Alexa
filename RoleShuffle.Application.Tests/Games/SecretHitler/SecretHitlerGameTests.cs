using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Games.OneNightUltimateWerewolf;
using RoleShuffle.Application.Games.SecretHitler;
using RoleShuffle.Application.SSMLResponses;
using SSMLVerifier;

namespace RoleShuffle.Application.Tests.Games.SecretHitler
{
    [TestClass]
    public class SecretHitlerGameTests : GameTestBase
    {
        [TestInitialize]
        public void TestFixture()
        {
            var game = new SecretHitlerGame();
            Initialize(game);
        }

        [TestMethod]
        public async Task ChososeNumberBetweenReturnsValidSSML()
        {
            const short minPlayers = 5;
            const short maxPlayers = 10;
            var ssml = await CommonResponseCreator.GetGameSpecificSSMLAsync(
                "SecretHitler",
                "ChoosePlayerNumberBetween",
                "de-DE",
                new[]
                {
                    minPlayers,
                    maxPlayers
                });

            var ssmlValidationErrors = Verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }
    }
}