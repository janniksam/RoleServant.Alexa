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
            const short MinPlayers = 5;
            const short MaxPlayers = 10;
            var ssml = await CommonResponseCreator.GetSSMLAsync(typeof(OneNightUltimateWerewolfRound),
                typeof(SecretHitlerGame).Namespace + ".SSMLViews", "ChoosePlayerNumberBetween", "de-DE",
                new[]
                {
                    MinPlayers,
                    MaxPlayers
                });

            var ssmlValidationErrors = Verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }
    }
}