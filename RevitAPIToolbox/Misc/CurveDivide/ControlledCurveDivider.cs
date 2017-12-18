using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Techyard.Revit.Attributes;
using Techyard.Revit.Database;

namespace Techyard.Revit.Misc.CurveDivide
{
    [TargetType(typeof(HermiteSpline), typeof(NurbSpline), typeof(CylindricalHelix), typeof(Ellipse))]
    internal class ControlledCurveDivider : CurveDivider
    {
        internal override IEnumerable<XYZ> Divide(Curve curve, int number)
        {
            var length = curve.Length;
            var lengthEach = length / number;
            var accumulatedLength = 0D;
            XYZ lastPoint = null;
            return base.Divide(curve, number * 10).Take(number * 10 - 1).Select(point =>
            {
                if (null == lastPoint)
                {
                    lastPoint = point;
                    return null;
                }
                var tempLength = point.DistanceTo(lastPoint);
                if (accumulatedLength + tempLength < lengthEach)
                {
                    accumulatedLength += tempLength;
                    return null;
                }
                var result = lastPoint.LinearInterpolation(point, lengthEach - accumulatedLength);
                accumulatedLength = accumulatedLength + tempLength - lengthEach;
                return result;
            }).Where(point => point != null);
        }
    }
}