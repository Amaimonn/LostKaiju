using UnityEngine;

namespace LostKaiju.Utils
{
    public static class LocaleHelper
    {
        public const string EN = "en";
        public const string RU = "ru";

        public static int GetLanguageIndexByCode(string code)
        {
            int index = code switch
            {
                EN => 0,
                RU => 1,
                _ => -1,
            };

            if (index == -1)
            {
                Debug.LogWarning($"Unknown language code: {code}");
                return 0;
            }
            else
            {
                return index;
            }
        }

        public static string GetLanguageCodeByIndex(int index)
        {
            string code = index switch
            {
                0 => EN,
                1 => RU,
                _ => null,
            };

            if (code == null)
            {
                Debug.LogWarning($"Unknown language index: {index}");
                return EN;
            }
            else
            {
                return code;
            }
        }
    }
}