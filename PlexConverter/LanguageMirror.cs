using System;
using System.Collections.Generic;
using System.Text;

namespace PlexConverter
{
    public static class LanguageMirror
    {
        private static readonly Dictionary<string, string> _langs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"Romanian", "ro" },
            {"English", "en" },
            {"Spanish", "es" },

            {"Moldavian", "mo" },
            {"Ukrainian", "uk" },
            {"Hungarian", "hu" },
            {"Serbian", "sr" },
            {"Bulgarian", "bg" },
            {"Albanian", "sq" },
            {"Macedonian", "mk" },
            {"Greek", "el" },
            {"Turkish", "tr" },
            {"Maltese", "mt" },
            {"Bosnian", "bs" },
            {"Slovenian", "sl" },
            {"Croatian", "hr" },
            {"German", "de" },
            {"Slovak", "sk" },
            {"Czech", "cs" },
            {"Polish", "pl" },
            {"Belarusian", "bl" },
            {"Lithuanian", "lt" },
            {"Latvian", "lv" },
            {"Estonian", "et" },
            {"Finnish", "fi" },
            {"Swedish", "sv" },
            {"Norwegian Bokmal", "nb" },
            {"Norwegian Nynorsk", "nn" },
            {"Norwegian", "no" },
            {"Danish", "da" },
            {"Romansh", "rm" },
            {"French", "fr" },
            {"Italian", "it" },
            {"Dutch", "nl" },
            {"Portuguese", "pt" },
            {"Irish", "ga" },
            {"Icelandic", "is" },
            {"Faroese", "fo" },
            {"Greenlandic", "kl" },
            {"Russian", "ru" },

            {"Chinese", "zh" },
            {"Japanese", "ja" },
            {"Korean", "ko" },
            {"Persian", "fa" },
            {"Hebrew", "he" },
            {"Arabic", "ar" }
        };
        public static string GetCode(string lang)
        {
            return _langs.TryGetValue(lang, out string code) ? code : "und";
        }
    }
}
