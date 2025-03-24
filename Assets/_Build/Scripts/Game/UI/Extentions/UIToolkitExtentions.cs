using System;
using R3;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.Extentions
{
    public static class UIToolkitExtentions
    {
        public static IDisposable LocalizeText(this Label label, string table, string entry)
        {
            var localizedString = new LocalizedString(table, entry);
            localizedString.StringChanged += LocalizeLabelCallback;
            
            return Disposable.Create(() => localizedString.StringChanged -= LocalizeLabelCallback);

            void LocalizeLabelCallback(string value)
            {
                label.text = value;
            }
        }

        // public static void LocalizeLabel(this Tab tab, string table, string entry)
        // {
        //     tab.SetBinding("label", new LocalizedString(table, entry));
        // }

        public static IDisposable LocalizeText(this Button button, string table, string entry)
        {
            var localizedString = new LocalizedString(table, entry);
            localizedString.StringChanged += LocalizeButtonCallback;

            return Disposable.Create(() => localizedString.StringChanged -= LocalizeButtonCallback);

            void LocalizeButtonCallback(string value)
            {
                button.text = value;
            }
        }
    }
}