using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class ParameterExtension
    {
        public static object GetValue(this Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsDouble();
                case StorageType.ElementId:
                    return parameter.AsElementId();
                case StorageType.Integer:
                    return parameter.AsInteger();
                case StorageType.String:
                    return parameter.AsString();
                default:
                    return parameter.AsValueString();
            }
        }
    }
}
