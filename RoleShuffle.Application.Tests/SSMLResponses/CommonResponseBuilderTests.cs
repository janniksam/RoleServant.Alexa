using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Games.Insider;
using RoleShuffle.Application.Games.TheResistanceAvalon;
using RoleShuffle.Application.SSMLResponses;
using SSMLVerifier;

namespace RoleShuffle.Application.Tests.SSMLResponses
{
    [TestClass]
    public class CommonResponseBuilderTests
    {
        [TestMethod]
        public async Task EveryLanguageFolderShouldContainTheSSMLViewsForEveryMessageKey()
        {
            var namespacePrefix = $"{typeof(CommonResponseCreator).Namespace}.Common";

            var locatedResources = typeof(CommonResponseCreator).GetTypeInfo().Assembly.GetManifestResourceNames()
                .Where(p => p.StartsWith(namespacePrefix)).ToList();

            Assert.AreNotEqual(0, locatedResources.Count);

            var regex = new Regex($"^.+[{namespacePrefix}]\\.(.+)\\..+\\..+$");
            var locatedLocales = locatedResources.Where(p => regex.IsMatch(p)).Select(p => regex.Match(p).Groups[1].Captures[0].Value).Distinct().ToList();
            
            var requiredViewNames = GetConstants(typeof(MessageKeys)).Select(p => p.GetRawConstantValue().ToString()).ToList();
            foreach (var requiredViewName in requiredViewNames)
            {
                foreach (var locale in locatedLocales.Where(p => p != "All"))
                {
                    var shouldBeThere = $"{namespacePrefix}.{locale}.{requiredViewName}.cshtml";
                    Assert.AreEqual(
                        true, 
                        locatedResources.Any(p => p.Equals(shouldBeThere)),
                        $"{shouldBeThere} not found.");
                }
            }

            var verifier = new Verifier();
            foreach (var requiredViewName in requiredViewNames)
            {
                foreach (var locale in locatedLocales.Where(p => p != "All"))
                {
                    var toTest = $"{namespacePrefix}.{locale}.{requiredViewName}.cshtml";
                    var ssml = await CommonResponseCreator.GetSSMLAsync(requiredViewName, locale);
                    var errors = verifier.Verify(ssml, SsmlPlatform.Amazon);
                    Assert.AreEqual(0, errors.Count(), $"{toTest} doesn't return valid SSML.");
                }
            }
        }

        [TestMethod]
        public void GeneralLanguageFolderShouldContainTheSSMLViewsForEveryAllLanguagesMessageKey()
        {
            var namespacePrefix = $"{typeof(CommonResponseCreator).Namespace}.Common";

            var locatedResources = typeof(CommonResponseCreator).GetTypeInfo().Assembly.GetManifestResourceNames()
                .Where(p => p.StartsWith(namespacePrefix)).ToList();

            Assert.AreNotEqual(0, locatedResources.Count);

            var regex = new Regex($"^.+[{namespacePrefix}]\\.(.+)\\..+\\..+$");
            var locatedLocales = locatedResources.Where(p => regex.IsMatch(p)).Select(p => regex.Match(p).Groups[1].Captures[0].Value).Distinct().ToList();

            var requiredViewNames = GetConstants(typeof(MessageKeys.AllLanguages)).Select(p => p.GetRawConstantValue());
            foreach (var requiredViewName in requiredViewNames)
            {
                foreach (var locale in locatedLocales.Where(p => p == "All"))
                {
                    var shouldBeThere = $"{namespacePrefix}.{locale}.{requiredViewName}.cshtml";
                    Assert.AreEqual(
                        true,
                        locatedResources.Any(p => p.Equals(shouldBeThere)),
                        $"{shouldBeThere} not found.");
                }
            }
        }

        [TestMethod]
        public async Task ShouldGetValidSSMLForErrorMalformedRequest()
        {
            var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.AllLanguages.ErrorMalformedRequest, "All");
            var verifier = new Verifier();
            var ssmlValidationErrors = verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        [TestMethod]
        public async Task ShouldGetValidSSMLForGermanHelpMessage()
        {
            var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.HelpMessage, "de-DE");
            var verifier = new Verifier();
            var ssmlValidationErrors = verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }


        [TestMethod]
        public async Task ShouldGetValidSSMLForGermanChooseGameMessage()
        {
            var ssml = await CommonResponseCreator.GetSSMLAsync(MessageKeys.ChooseGame, "de-DE",
                new List<IGame> {new TheResistanceAvalonGame(), new InsiderGame()});
            var verifier = new Verifier();
            var ssmlValidationErrors = verifier.Verify(ssml, SsmlPlatform.Amazon);
            Assert.AreEqual(0, ssmlValidationErrors.Count());
        }

        private static IEnumerable<FieldInfo> GetConstants(Type type)
        {
            var fieldInfos = type.GetFields(BindingFlags.Public |
                                                    BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }
    }
}
