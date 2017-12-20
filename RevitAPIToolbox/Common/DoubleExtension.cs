using Autodesk.Revit.DB;

namespace Techyard.Revit.Common
{
    internal static class DoubleExtension
    {
        /// <summary>
        ///     Convert a value in millimeter to revit internal unit
        /// </summary>
        /// <param name="millimeter">Value in millimeter</param>
        /// <returns>Value in feet</returns>
        internal static double MillimeterToInternal(this double millimeter)
        {
            return millimeter.ToInternal(DisplayUnitType.DUT_MILLIMETERS);
        }

        /// <summary>
        ///     Convert a value in square millimeter to revit internal unit
        /// </summary>
        /// <param name="squareMillimeter">Value in square millimeter</param>
        /// <returns>Value in square feet</returns>
        internal static double SquareMillimeterToInternal(this double squareMillimeter)
        {
            return squareMillimeter.ToInternal(DisplayUnitType.DUT_SQUARE_MILLIMETERS);
        }

        /// <summary>
        ///     Convert a value in cube millimeter to revit internal unit
        /// </summary>
        /// <param name="cubeMillimeter">Value in cube millimeter</param>
        /// <returns>Value in cube feet</returns>
        internal static double CubeMillimeterToInternal(this double cubeMillimeter)
        {
            return cubeMillimeter.ToInternal(DisplayUnitType.DUT_CUBIC_MILLIMETERS);
        }

        /// <summary>
        ///     Convert a value to revit internal unit
        /// </summary>
        /// <param name="display">Value</param>
        /// <param name="unit">Unit</param>
        /// <returns>Value in revit internal unit</returns>
        internal static double ToInternal(this double display, DisplayUnitType unit)
        {
            return UnitUtils.ConvertToInternalUnits(display, unit);
        }
    }
}
