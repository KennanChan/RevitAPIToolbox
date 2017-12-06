using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Implementations.CurveWalker
{
    internal class RegularCurveWalker : CurveWalker
    {
        private double CurrentParameter { get; set; }

        public RegularCurveWalker(Curve curve, XYZ initPosition = null,
            WalkingDirection direction = WalkingDirection.StartToEnd) : base(curve, initPosition, direction)
        {
            GetCurrentParameter();
        }

        private void GetCurrentParameter()
        {
            CurrentParameter = WalkingCurve.Project(CurrentPosition).Parameter;
        }

        internal override bool Walk(double distance)
        {
            throw new NotImplementedException();
        }

        internal override bool WalkToEnd(double step, List<XYZ> result)
        {
            throw new NotImplementedException();
        }
    }
}