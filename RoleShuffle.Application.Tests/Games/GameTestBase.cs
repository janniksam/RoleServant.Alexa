using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.SSMLResponses;
using RoleShuffle.Base;
using SSMLVerifier;

namespace RoleShuffle.Application.Tests.Games
{
    public abstract class GameTestBase
    {
        protected IGame Game;
        protected Verifier Verifier;
        private SkillRequest m_request;

        protected void Initialize(IGame game)
        {
            Game = game;
            Verifier = new Verifier();
            m_request = new SkillRequest()
            {
                Session = new Session()
                {
                    Attributes = new Dictionary<string, object>
                    {
                        { "AskForPercival", Constants.SlotYesNoResult.Yes },
                        { "AskForMorgana", Constants.SlotYesNoResult.Yes },
                        { "AskForMordred", Constants.SlotYesNoResult.Yes },
                        { "AskForOberon", Constants.SlotYesNoResult.Yes }
                    }
                },
                Request = new IntentRequest
                {
                    Locale = "de-DE",
                    Intent = new Intent
                    {
                        Slots = new Dictionary<string, Slot>
                        {
                            { Constants.Slots.PlayerAmount, new Slot { Value = "5" } }
                        }
                    }
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
            await Game.StartGameRequested(m_request);
            var distributeRolesResponse = await Game.DistributeRoles(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)distributeRolesResponse.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = Verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnDistributeRolesWithoutActiveGame()
        {
            var distributeRolesResponse = await Game.DistributeRoles(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)distributeRolesResponse.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = Verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnNightPhase()
        {
            await Game.StartGameRequested(m_request);
            var nightphaseResponse = await Game.PerformNightPhase(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)nightphaseResponse.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = Verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnNightPhaseWithoutActiveGame()
        {
            var nightphaseResponse = await Game.PerformNightPhase(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)nightphaseResponse.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = Verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task DoesReturnValidSSMLOnStartGameRequested()
        {
            var distributeRoles = await Game.StartGameRequested(m_request);
            var responseOutputSpeech = ((SsmlOutputSpeech)distributeRoles.Response.OutputSpeech).Ssml;
            var ssmlValidationErrors = Verifier.Verify(responseOutputSpeech, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public void GameViewsAreAllThereForEveryLanguage()
        {
            var namespacePrefix = $"{typeof(CommonResponseCreator).Namespace}.{Game.SSMLViewFolder}";
            var allResources = Game.GetType()
                .GetTypeInfo().Assembly.GetManifestResourceNames()
                .Where(p => p.StartsWith(namespacePrefix)).ToList();

            var locatedLocales = Localization.GetLocalesFolderPaths();
            foreach (var locatedLocale in locatedLocales)
            {
                var  localePrefix = $"{namespacePrefix}.{locatedLocale}";
                var localizedResources = allResources.Where(p => p.StartsWith(localePrefix)).ToList();
                var requiredViews = Game.GetRequiredSSMLViews().Select(rv => $"{localePrefix}.{rv}.cshtml").ToList();

                // Check if all resources are there, that are required:
                foreach (var requiredView in requiredViews)
                {
                    Assert.IsTrue(localizedResources.Any(p => p.Equals(requiredView)), $"Missing resource '{requiredView}'");
                }

                // Check for unknown views that shouldnt be there:
                foreach (var locatedResource in localizedResources)
                {
                    Assert.IsTrue(requiredViews.Contains(locatedResource), $"Unexpected view '{locatedResource}' found");
                }
            }
        }
    }
}