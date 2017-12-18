using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Misc.CurveDivide
{
    internal interface ICurveDivider
    {
        IEnumerable<XYZ> EquallyDivide(Curve curve, int number);
    }
}