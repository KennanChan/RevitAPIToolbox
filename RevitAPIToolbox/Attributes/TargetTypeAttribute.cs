using System;

namespace Techyard.Revit.Attributes
{
    internal class TargetTypeAttribute : Attribute
    {
        internal Type[] Types { get; }

        internal TargetTypeAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}