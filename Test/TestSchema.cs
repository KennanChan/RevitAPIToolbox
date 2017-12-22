using System;
using System.Web.Script.Serialization;
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
            var mySchema = new CustomSchema { Editor = "Kennan", Age = 26 };
            element.WriteData(mySchema);
            var value = element.ReadData(new CustomSchema());
            TaskDialog.Show("Value", new JavaScriptSerializer().Serialize(value));
            return Result.Succeeded;
        }
    }

    [Schema(Name = "MySchema", Guid = "09A33462-4979-49F0-A15E-90DFE444C243")]
    public class CustomSchema : SchemaBase
    {
        [SchemaField(Name = "Editor")]
        public string Editor { get; set; }

        [SchemaField(Name = "Age")]
        public int Age { get; set; }
    }
}
