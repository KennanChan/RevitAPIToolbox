using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Techyard.Revit.Attributes;
using Techyard.Revit.Database;
using Techyard.Revit.Misc.SchemaDesigner;
using Techyard.Revit.UI;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class TestSchema : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiDocument = commandData.Application.ActiveUIDocument;
            var document = uiDocument.Document;
            var element = uiDocument.Selection.SelectSingle(document);
            var mySchema = new CustomSchema();
            element.WriteData(mySchema.GetOrCreate(), "Editor", "Kennan");
            var editor = element.ReadData<string>(mySchema.GetOrCreate(), "Editor");
            TaskDialog.Show("Editor", editor);
            return Result.Succeeded;
        }
    }

    [Schema(Name = "MySchema", Guid = "09A33462-4979-49F0-A15E-90DFE444C243")]
    public class CustomSchema : SchemaBase
    {
        [SchemaField(Name = "Editor")]
        public string Editor { get; set; }
    }
}
