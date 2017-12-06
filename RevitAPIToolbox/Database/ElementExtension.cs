using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class ElementExtension
    {
        public static ElementType GetElementType(this Element element)
        {
            return element.Document.GetElement(element.GetTypeId()) as ElementType;
        }
    }
}