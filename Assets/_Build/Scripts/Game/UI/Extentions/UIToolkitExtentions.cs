using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.Extentions
{
    public static class UIToolkitExtentions
    {
        public static void LocalizeText(this Label label, string table, string entry)
        {
            label.SetBinding("text", new LocalizedString(table, entry));
        }

        public static void LocalizeLabel(this Tab tab, string table, string entry)
        {
            tab.SetBinding("label", new LocalizedString(table, entry));
        }

        public static void LocalizeText(this Button button, string table, string entry)
        {
            button.SetBinding("text", new LocalizedString(table, entry));
        }
    }
}