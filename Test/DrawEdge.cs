using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Techyard.Revit.Common;
using Techyard.Revit.Geometry;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class DrawEdge : IExternalCommand
    {
        private IEnumerable<XYZ> Points { get; } = new[] { XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ };

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var app = commandData.Application;
            //Application.RegisterFailuresProcessor(new FailureProcessor());
            var uiDocument = app.ActiveUIDocument;
            var document = uiDocument.Document;
            using (var transaction = new Transaction(document, "draw"))
            {
                transaction.Start();
                var elementList = uiDocument.Selection.GetElementIds().Select(id => document.GetElement(id)).ToList();
                if (elementList.Count == 0)
                    elementList = new FilteredElementCollector(document)
                        .WherePasses(
                            new ElementMulticlassFilter(
                                new[]
                                {
                                    typeof(GenericForm),
                                    typeof(FamilyInstance)
                                }))
                        .ToList();
                elementList.ForEach(element =>
                {
                    try
                    {
                        var form = element as GenericForm;
                        if (null != form && !form.IsSolid)
                            return;
                        new FilteredGeometryCollector(element)
                            .OfClass<Solid>()
                            .Cast<Solid>()
                            .SelectMany(solid => solid.Edges.AsList<Edge>())
                            .ToList()
                            .ForEach(
                                edge =>
                                {
                                    try
                                    {
                                        var curve = edge.AsCurve();
                                        var sketchFace = edge.GetFace(0) as PlanarFace ?? edge.GetFace(1) as PlanarFace;
                                        var sketchPlane = SketchPlane.Create(document,
                                            null == sketchFace
                                                ? Plane.CreateByThreePoints(curve.GetEndPoint(0), curve.GetEndPoint(1),
                                                    Points.First(p => !p.IsAlmostEqualTo(curve.Project(p).XYZPoint)))
                                                : Plane.CreateByNormalAndOrigin(sketchFace.FaceNormal, sketchFace.Origin));
                                        document.FamilyCreate.NewModelCurve(curve, sketchPlane);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                });

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
                transaction.Commit();
            }
            return Result.Succeeded;
        }
    }

    public class FailureProcessor : IFailuresProcessor
    {
        public FailureProcessingResult ProcessFailures(FailuresAccessor data)
        {
            switch (data.GetSeverity())
            {
                case FailureSeverity.Warning:
                    data.ResolveFailures(data.GetFailureMessages());
                    return FailureProcessingResult.ProceedWithCommit;
                default:
                    return FailureProcessingResult.ProceedWithRollBack;
            }
        }

        public void Dismiss(Document document)
        {

        }
    }
}
