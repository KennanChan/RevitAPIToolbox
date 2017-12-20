using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Geometry
{
    public static class GeometryElementExtension
    {
        public static IEnumerable<GeometryObject> GetObjects(this GeometryElement ge)
        {
            return ge.SelectMany(go =>
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
