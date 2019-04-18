using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Games.Insider;
using SSMLVerifier;

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
    }
}