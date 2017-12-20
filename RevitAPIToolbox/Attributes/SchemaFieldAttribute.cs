using System;

namespace Techyard.Revit.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SchemaFieldAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
