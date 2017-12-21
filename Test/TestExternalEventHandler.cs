using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Techyard.Revit.Common;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class TestExternalEventHandler : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var handler = new CustomEventHandler2();
            var event2 = ExternalEvent.Create(handler);
            if (ExternalEventManager.Manager.Register(new CustomEventHandler()))
            {
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("刘一");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("陈二");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("张三");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("李四");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("王五");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("赵六");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("孙七");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("周八");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("吴九");
                event2.Raise();
                ExternalEventManager.Manager.Raise<CustomEventHandler, string>("郑十");
                event2.Raise();
            }
            return Result.Succeeded;
        }

        private class CustomEventHandler : ParameterizedExternalEventHandler<string>
        {
            private int Count { get; set; }

            public override void Execute(UIApplication app, string parameter)
            {
                TaskDialog.Show("外部事件", $"{GetName()}事件第{++Count}次调用:{parameter}");
            }

            public override string GetName()
            {
                return "Test";
            }
        }

        private class CustomEventHandler2 : IExternalEventHandler
        {
            private int Count { get; set; }

            public void Execute(UIApplication app)
            {
                TaskDialog.Show("外部事件", $"{GetName()}事件第{++Count}次调用");
            }

            public string GetName()
            {
                return "Test";
            }
        }
    }
}
