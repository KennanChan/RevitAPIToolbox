using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Implementations.CurveWalker
{
    internal abstract class CurveWalker
    {
        protected CurveWalker(Curve curve, XYZ initPosition = null,
            WalkingDirection direction = WalkingDirection.StartToEnd)
        {
            WalkingCurve = curve;
            CurrentPosition = initPosition ?? WalkingCurve.GetEndPoint(0);
            Direction = direction;
        }

        internal Curve WalkingCurve { get; }

        internal XYZ CurrentPosition { get; private set; }

        internal WalkingDirection Direction { get; private set; }

        internal abstract bool Walk(double distance);

        internal IEnumerable<XYZ> WalkToEnd(double step)
        {
            while (Walk(step))
            {
                yield return CurrentPosition;
            }
        }

        internal bool WalkBack(double distance)
        {
            Direction = (WalkingDirection)(0 - (short)Direction);
            return Walk(distance);
        }
    }
}