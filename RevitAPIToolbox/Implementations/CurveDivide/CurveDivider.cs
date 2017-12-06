using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB;
using Techyard.Revit.Attributes;
using Techyard.Revit.Common;

namespace Techyard.Revit.Implementations.CurveDivide
{
    [TargetType(typeof(Line), typeof(Arc))]
    internal class CurveDivider
    {
        internal virtual IEnumerable<XYZ> Divide(Curve curve, int number)
        {
            return number.TraverseFrom(1, x => curve.Evaluate(1D / x, true)).ToList();
        }

        private static IDictionary<Type, ICurveDivider> Dividers { get; } =
            new ConcurrentDictionary<Type, ICurveDivider>();

        internal static ICurveDivider GetDivider(Type curveType)
        {
            if (Dividers.ContainsKey(curveType))
                return Dividers[curveType];
            var dividerType =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(
                        t => t.GetCustomAttributes(typeof(TargetTypeAttribute), false)
                                 .Cast<TargetTypeAttribute>()
                                 .FirstOrDefault(attribute => attribute.Types.Contains(curveType)) != null);
            if (null == dividerType) throw new Exception("No divider defined to handle this kind of Curve");
            var divider = Activator.CreateInstance(dividerType) as ICurveDivider;
            if (null == divider) throw new Exception($"Fail to create instance of {dividerType.FullName}");
            Dividers.Add(curveType, divider);
            return divider;
        }
    }
}