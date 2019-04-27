using System;
using System.Collections.Generic;
using System.Linq;
namespace RoleShuffle.Application
{
    public class Localization
    {
        private static readonly Dictionary<string,string> m_supportedLocales = new Dictionary<string, string>
        {
            { "de-DE", "de_DE" },
            { "en-AU", "en_US" },
            { "en-CA", "en_US" },
            { "en-GB", "en_US" },
            { "en-IN", "en_US" },
            { "en-US", "en_US" },
        };

        public static string MapLocaleToFolderPath(string locale)
        {
            if (locale == "All")
            {
                return locale;
            }

            if (!m_supportedLocales.TryGetValue(locale, out string folderPath))
            {
                throw new NotSupportedException($"Locale {locale} is currently not supported.");
            }

            return folderPath;
        }

        public static bool IsSupported(string locale)
        {
            return m_supportedLocales.ContainsKey(locale);
        }

        public static string[] GetLocalesFolderPaths()
        {
            return m_supportedLocales.Select(p => p.Value).Distinct().ToArray();
        }

        public static string[] GetSupportedLocales()
        {
            return m_supportedLocales.Select(p => p.Key).Distinct().ToArray();
        }
    }
}