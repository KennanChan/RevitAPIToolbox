using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Geometry
{
    public static class GeometryInstanceExtension
    {
        public static IEnumerable<GeometryObject> GetObjects(this GeometryInstance gi)
        {
            return gi.GetInstanceGeometry().SelectMany(go =>
            {
                switch (go)
                {
                    case GeometryElement _:
                        return (go as GeometryElement).GetObjects();
                    case GeometryInstance _:
                        return (go as GeometryInstance).GetObjects();
                }
                return new[] {go};
            });
        }
    }
}
