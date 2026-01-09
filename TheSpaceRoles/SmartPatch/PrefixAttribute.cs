using System;

namespace TSR;

[AttributeUsage(AttributeTargets.Method)]
public class SmartPrefixAttribute : Attribute
{
    public SmartPrefixAttribute()
    { }
}