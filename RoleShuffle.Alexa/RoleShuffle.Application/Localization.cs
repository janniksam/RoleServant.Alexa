using System;
using System.Collections.Generic;
using System.Linq;
namespace RoleShuffle.Application
{
    public class Localization
    {
        private static readonly Dictionary<string,string> SupportedLocales = new Dictionary<string, string>
        {
            { "de-DE", "de_DE" },
            { "en-US", "en_US" },
        };

        public static string MapLocaleToFolderPath(string locale)
        {
            if (locale == "All")
            {
                return locale;
            }

            if (!SupportedLocales.TryGetValue(locale, out string folderPath))
            {
                throw new NotSupportedException($"Locale {locale} is currently not supported.");
            }

            return folderPath;
        }

        public static bool IsSupported(string locale)
        {
            return SupportedLocales.ContainsKey(locale);
        }

        public static string[] GetLocalesFolderPaths()
        {
            return SupportedLocales.Select(p => p.Value).Distinct().ToArray();
        }

        public static string[] GetSupportedLocales()
        {
            return SupportedLocales.Select(p => p.Key).Distinct().ToArray();
        }
    }
}