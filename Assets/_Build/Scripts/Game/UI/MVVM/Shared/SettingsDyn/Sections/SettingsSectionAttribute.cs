using System;

namespace LostKaiju.Game.UI.MVVM.Shared.SettingsDyn
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class SettingsSectionAttribute : Attribute
    {
        public readonly string SectionId;

        public SettingsSectionAttribute(string sectionId)
        {
            SectionId = sectionId;
        }
    }
}