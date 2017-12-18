using System;

namespace Techyard.Revit.Attributes
{
    internal class TargetTypeAttribute : Attribute
    {
        internal TargetTypeAttribute(params Type[] types)
        {
            Types = types;
        }

        internal Type[] Types { get; }
    }
}