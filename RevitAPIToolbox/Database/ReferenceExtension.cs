using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class ReferenceExtension
    {
        public static Element ToElement(this Reference reference,Document document)
        {
            return document.GetElement(reference);
        }
    }
}
