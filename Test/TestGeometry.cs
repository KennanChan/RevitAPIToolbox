using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Techyard.Revit.Geometry;
using Techyard.Revit.UI;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class TestGeometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDocument = commandData.Application.ActiveUIDocument;
            var document = uiDocument.Document;
            var element = uiDocument.Selection.SelectSingle(document);
            var collector = new FilteredGeometryCollector(element).OfClass<Solid>().Cast<Solid>();
            TaskDialog.Show($"Total:{collector.Count()}", $"{collector.Sum(solid => solid.Volume)}");
            return Result.Succeeded;
        }
    }
}