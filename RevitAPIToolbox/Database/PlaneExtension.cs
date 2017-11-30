using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class PlaneExtension
    {
        /// <summary>
        ///     Intersects the Line with current plane
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="line"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static SetComparisonResult Intersect(this Plane plane, Line line, out IntersectionResultArray result)
        {
            var startPojection = plane.Project(line.GetEndPoint(0));
            var endProjection = plane.Project(line.GetEndPoint(1));
            var projectionLine = line.IsBound
                ? Line.CreateBound(startPojection, endProjection)
                : Line.CreateUnbound(startPojection, endProjection - startPojection);
            return line.Intersect(projectionLine, out result);
        }

        public static bool IsPointResideIn(this Plane plane, XYZ point)
        {
            return point.IsAlmostEqualTo(plane.Origin) || (point - plane.Origin).DotProduct(plane.Normal).Equals(0);
        }

        public static XYZ Project(this Plane plane, XYZ point)
        {
            if (plane.IsPointResideIn(point)) return point;
            using (var transform = Transform.Identity)
            {
                transform.BasisX = plane.XVec;
                transform.BasisY = plane.YVec;
                transform.BasisZ = plane.Normal;
                transform.Origin = plane.Origin;
                var pointInPlaneCoordinate = transform.Inverse.OfPoint(point);
                var pointOnPlane = new XYZ(pointInPlaneCoordinate.X, pointInPlaneCoordinate.Y, 0);
                return transform.OfPoint(pointOnPlane);
            }
        }
    }
}
