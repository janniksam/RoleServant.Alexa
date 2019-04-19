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
        private static readonly string m_defaultNamespace = $"{typeof(CommonResponseCreator).Namespace}.Common";

        public static Task<string> GetSSMLAsync(string messageKey, string locale, object model = null)
        {
            var ssmlManifestResourceKey = $"{m_defaultNamespace}.{locale.Replace("-", "_")}.{messageKey}.cshtml";
            var ssmlStream = GetSSMLStream(m_defaultType, ssmlManifestResourceKey);
            return ConvertTemplate(ssmlStream, ssmlManifestResourceKey, model);
        }

        public static Task<string> GetSSMLAsync(Type type, string resourceNamespace, string messageKey, string locale, object model = null)
        {
            var ssmlManifestResourceKey = $"{resourceNamespace}.{locale.Replace("-", "_")}.{messageKey}.cshtml";
            var ssmlStream = GetSSMLStream(type, ssmlManifestResourceKey);
            return ConvertTemplate(ssmlStream, ssmlManifestResourceKey, model);
        }

        private static Task<string> ConvertTemplate(Stream ssmlStream, string templateKey, object model = null)
        {
            return SSMLRazorBuilder.BuildFromAsync(ssmlStream, templateKey, model);
        }

        private static string GetTemplateKey(string messageKey, string locale)
        {
            if (messageKey == null)
            {
                throw new ArgumentNullException(nameof(messageKey));
            }
            if (locale == null)
            {
                throw new ArgumentNullException(nameof(locale));
            }
            return $"{messageKey}_{locale}";
        }

        private static Stream GetSSMLStream(Type type, string ssmlManifestResourceKey)
        {
            var assembly = type.GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(ssmlManifestResourceKey);
            return resource;
        }
    }
}