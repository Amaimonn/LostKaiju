using System;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class SettingMethod : Attribute
{
    public readonly string MethodTag;

    public SettingMethod(string methodTag)
    {
        MethodTag = methodTag;
    }
}
