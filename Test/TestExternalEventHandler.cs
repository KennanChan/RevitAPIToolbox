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
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("刘一",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("陈二",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("张三",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("李四",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("王五",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("赵六",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("孙七",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("周八",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("吴九",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            ExternalEvent.Create(new DelegateExternalEventHandler<string>("郑十",
                (app, p) => { TaskDialog.Show("外部事件", $"调用:{p}"); })).Raise();
            TaskDialog.Show("Message", "你会先看到这一条");
            return Result.Succeeded;
        }
    }
}
