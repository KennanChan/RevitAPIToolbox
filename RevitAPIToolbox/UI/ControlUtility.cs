namespace Techyard.Revit.UI
{
    internal class ControlUtility
    {
        internal static string GenerateId(string parentId, string name)
        {
            return $"CustomCtrl_%{parentId}%{name}";
        }
    }
}
