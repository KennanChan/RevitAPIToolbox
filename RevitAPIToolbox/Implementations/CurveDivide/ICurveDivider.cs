using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Implementations.CurveDivide
{
    internal interface ICurveDivider
    {
        IEnumerable<XYZ> EquallyDivide(Curve curve, int number);
    }
}