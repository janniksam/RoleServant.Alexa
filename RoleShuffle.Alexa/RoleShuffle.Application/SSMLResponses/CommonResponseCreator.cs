using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using SSMLBuilder;

namespace RoleShuffle.Application.SSMLResponses
{
    public static class CommonResponseCreator
    {
        private static readonly Type m_defaultType = typeof(CommonResponseCreator);
        private static readonly string m_defaultNamespace = $"{typeof(CommonResponseCreator).Namespace}";

        public static Task<string> GetSSMLAsync(string messageKey, string locale, object model = null)
        {
            var ssmlManifestResourceKey = $"{m_defaultNamespace}.Common.{locale.Replace("-", "_")}.{messageKey}.cshtml";
            var ssmlStream = GetSSMLStream(m_defaultType, ssmlManifestResourceKey);
            return ConvertTemplate(ssmlStream, ssmlManifestResourceKey, model);
        }

        public static Task<string> GetGameSpecificSSMLAsync(string gameFolder, string messageKey, string locale, object model)
        {
            var ssmlManifestResourceKey = $"{m_defaultNamespace}.{gameFolder}.{locale.Replace("-", "_")}.{messageKey}.cshtml";
            var ssmlStream = GetSSMLStream(m_defaultType, ssmlManifestResourceKey);
            return ConvertTemplate(ssmlStream, ssmlManifestResourceKey, model);
        }

        private static Task<string> ConvertTemplate(Stream ssmlStream, string templateKey, object model = null)
        {
            return SSMLRazorBuilder.BuildFromAsync(ssmlStream, templateKey, model);
        }

        private static Stream GetSSMLStream(Type type, string ssmlManifestResourceKey)
        {
            var assembly = type.GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(ssmlManifestResourceKey);
            return resource;
        }
    }
}