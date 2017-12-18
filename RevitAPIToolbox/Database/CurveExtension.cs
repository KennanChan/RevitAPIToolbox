using System.Collections.Generic;
using Autodesk.Revit.DB;
using Techyard.Revit.Misc.CurveDivide;

namespace Techyard.Revit.Database
{
    public static class CurveExtension
    {
        public static IEnumerable<XYZ> EquallyDivideByInterpolation(this Curve curve, int divideNum)
        {
            return CurveDivider.GetDivider(curve.GetType()).EquallyDivide(curve, divideNum);
        }
    }
}