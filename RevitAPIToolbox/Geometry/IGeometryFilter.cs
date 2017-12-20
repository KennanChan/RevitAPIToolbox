using Autodesk.Revit.DB;

namespace Techyard.Revit.Geometry
{
    public interface IGeometryFilter
    {
        bool Filter(GeometryObject go);
    }
}
