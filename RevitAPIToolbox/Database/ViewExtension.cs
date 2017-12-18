using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Database
{
    public static class ViewExtension
    {
        /// <summary>
        ///     Hide a collection of elements in a specific view
        /// </summary>
        /// <param name="view">A view where the elements is to hide</param>
        /// <param name="elements">A collection of elements to hide</param>
        public static void HideElementsInView(this View view, IEnumerable<Element> elements)
        {
            try
            {
                if (null == view)
                    return;
                var document = view.Document;
                var elementsToHide = elements
                    .Where(element => element.Id != view.Id)
                    .Where(element => element.CanBeHidden(view))
                    .Select(element => element.Id).ToArray();
                if (!elementsToHide.Any())
                    return;
                using (var transaction = new Transaction(document, "Hide Elements"))
                {
                    transaction.Start();
                    view.HideElements(elementsToHide);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}