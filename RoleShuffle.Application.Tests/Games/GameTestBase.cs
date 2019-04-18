using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Abstractions.Games;
using SSMLVerifier;

namespace RoleShuffle.Application.Tests.Games
{
    public abstract class GameTestBase
    {
        private IGame m_game;
        private Verifier m_verifier;
        private SkillRequest m_request;

        protected void Initialize(IGame game)
        {
            m_game = game;
            m_verifier = new Verifier();
            m_request = new SkillRequest()
            {
                Request = new IntentRequest
                {
                    Locale = "de-DE"
                },
                Context = new Context
                {
                    System = new AlexaSystem
                    {
                        User = new User
                        {
                            UserId = "123"
                        }
                    }
                }
            };
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnDistributeRoles()
        {
            await m_game.StartGameRequested(m_request);
            var distributeRolesResponse = await m_game.DistributeRoles(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)distributeRolesResponse.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = m_verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnDistributeRolesWithoutActiveGame()
        {
            var distributeRolesResponse = await m_game.DistributeRoles(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)distributeRolesResponse.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = m_verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnNightPhase()
        {
            await m_game.StartGameRequested(m_request);
            var nightphaseResponse = await m_game.PerformNightPhase(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)nightphaseResponse.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = m_verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnNightPhaseWithoutActiveGame()
        {
            var nightphaseResponse = await m_game.PerformNightPhase(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)nightphaseResponse.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = m_verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnStartGameRequested()
        {
            var distributeRoles = await m_game.StartGameRequested(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)distributeRoles.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = m_verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }
    }
}