using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class XYZExtension
    {
        public static XYZ LinearInterpolation(this XYZ first, XYZ second, double length2First)
        {
            var vector = second - first;
            return first + vector * (length2First / vector.GetLength());
        }

        public static XYZ Copy(this XYZ xyz)
        {
            return new XYZ(xyz.X, xyz.Y, xyz.Z);
        }

        public static string AsString(this XYZ point)
        {
            return $"( {point.X} , {point.Y} , {point.Z} )";
        }
    }
}