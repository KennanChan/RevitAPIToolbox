using System;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Geometry
{
    public class GeometryClassFilter : IGeometryFilter
    {
        private Type Type { get; }

        public GeometryClassFilter(Type type)
        {
            Type = type;
        }

        public bool Filter(GeometryObject go)
        {
            return go.GetType() == Type;
        }
    }
}
