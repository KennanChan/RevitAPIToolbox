using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class PlaneExtension
    {
        /// <summary>
        ///     Intersects the Line with current plane
        /// </summary>
        /// <param name="plane">Invoking point</param>
        /// <param name="line">Target line</param>
        /// <param name="result">Intersection result object</param>
        /// <returns>Intersection status</returns>
        public static SetComparisonResult Intersect(this Plane plane, Line line, out IntersectionResultArray result)
        {
            var startPojection = plane.Project(line.GetEndPoint(0));
            var endProjection = plane.Project(line.GetEndPoint(1));
            var projectionLine = line.IsBound
                ? Line.CreateBound(startPojection, endProjection)
                : Line.CreateUnbound(startPojection, endProjection - startPojection);
            return line.Intersect(projectionLine, out result);
        }

        /// <summary>
        ///     Get a value indicating that a point resides
        /// </summary>
        /// <param name="plane">Invoking plane</param>
        /// <param name="point">Target point</param>
        /// <returns>A value indicating whether the point is inside the plane</returns>
        public static bool ContainsPoint(this Plane plane, XYZ point)
        {
            return point.IsAlmostEqualTo(plane.Origin) || (point - plane.Origin).DotProduct(plane.Normal).Equals(0);
        }

        /// <summary>
        ///     Project the point on the plane
        /// </summary>
        /// <param name="plane">Invoking plane</param>
        /// <param name="point">Target point</param>
        /// <returns>Projection point</returns>
        public static XYZ Project(this Plane plane, XYZ point)
        {
            if (plane.ContainsPoint(point)) return point;
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
