using System;

namespace TSR;

[AttributeUsage(AttributeTargets.Method)]
public class SmartPostfixAttribute : Attribute
{
    public SmartPostfixAttribute()
    { }
}