using System;
using System.Linq;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class DocumentExtension
    {
        public static View3D New3DView(this Document document, string name)
        {
            var viewFamilyType =
                new FilteredElementCollector(document).OfClass(typeof(ViewFamilyType))
                    .Cast<ViewFamilyType>()
                    .FirstOrDefault(type => type.ViewFamily == ViewFamily.ThreeDimensional);
            return null == viewFamilyType ? null : View3D.CreateIsometric(document, viewFamilyType.Id);
        }

        public static bool Modify(this Document document, Action action)
        {
            return document.Modify(action, Guid.NewGuid().ToString());
        }

        public static bool Modify(this Document document, Action action, string purpose)
        {
            using (var transaction = new Transaction(document, purpose))
            {
                try
                {
                    transaction.Start();
                    action?.Invoke();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.RollBack();
                    return false;
                }
            }
        }
    }
}