using System;

namespace Techyard.Revit.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SchemaAttribute : Attribute
    {
        public string Guid { get; set; }
        public string Name { get; set; }
    }
}
