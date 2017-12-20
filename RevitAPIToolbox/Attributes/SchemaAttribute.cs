using System;

namespace Techyard.Revit.Attributes
{
    public class SchemaAttribute : Attribute
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}
