using System;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Implementations.CurveWalker
{
    internal class ControlledCurveWalker : CurveWalker
    {
        internal ControlledCurveWalker(Curve curve, XYZ initPosition = null,
            WalkingDirection direction = WalkingDirection.StartToEnd) : base(curve, initPosition, direction)
        {
        }

        internal override bool Walk(double distance)
        {
            throw new NotImplementedException();
        }
    }
}