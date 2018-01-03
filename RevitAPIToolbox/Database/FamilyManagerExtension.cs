
using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class FamilyManagerExtension
    {
        public static void SetParameter(this FamilyManager manager, FamilyParameter parameter, object value)
        {
            if (null == parameter) return;
            if (parameter.IsReadOnly || parameter.IsDeterminedByFormula) return;
            if (value is ElementId)
                manager.Set(parameter, (ElementId)value);
            else if (value is int)
                manager.Set(parameter, (int)value);
            else if (value is double)
                manager.Set(parameter, (double)value);
            else if (value is string)
                manager.Set(parameter, (string)value);
        }
    }
}
