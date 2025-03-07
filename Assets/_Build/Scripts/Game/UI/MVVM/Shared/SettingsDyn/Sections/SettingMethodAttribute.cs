using System;

namespace LostKaiju.Game.UI.MVVM.Shared.SettingsDyn
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class SettingMethodAttribute : Attribute
    {
        public readonly string MethodId;

        public SettingMethodAttribute(string methodId)
        {
            MethodId = methodId;
        }
    }
}
