using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using SSMLBuilder;

namespace RoleShuffle.Application.SSMLResponses
{
    public static class CommonResponseCreator
    {
        private static Type DefaultType = typeof(CommonResponseCreator);
        private static string DefaultNamespace = $"{typeof(CommonResponseCreator).Namespace}.Common";


        public static Task<string> GetSSMLAsync(string messageKey, string locale, object model = null)
        {
            var templateKey = GetTemplateKey(messageKey, locale);
            var ssmlStream = GetSSMLStream(DefaultType, DefaultNamespace, messageKey, locale);
            return ConvertTemplate(ssmlStream, templateKey, model);
        }

        public static Task<string> GetSSMLAsync(Type type, string resourceNamespace, string messageKey, string locale, object model = null)
        {
            var templateKey = GetTemplateKey(messageKey, locale);
            var ssmlStream = GetSSMLStream(type, resourceNamespace, messageKey, locale);
            return ConvertTemplate(ssmlStream, templateKey, model);
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

        private static Stream GetSSMLStream(Type type, string resourceNamespace, string action, string locale)
        {
            var ssmlManifestResourceKey = $"{resourceNamespace}.{locale.Replace("-", "_")}.{action}.cshtml";
            var assembly = type.GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(ssmlManifestResourceKey);
            return resource;
        }
    }
}