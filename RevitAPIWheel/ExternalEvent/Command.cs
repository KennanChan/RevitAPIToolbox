using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Techyard.Revit.Wheel.ExternalEvent
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var event1 = ExternalEventWheel.Create(new ExternalEventHandlerDemo("张三"));
            var event2 = ExternalEventWheel.Create(new ExternalEventHandlerDemo("李四"));
            for (var i = 0; i < 3; i++)
            {
                //Events won't execute immediately
                //事件不会立即执行
                event1.Raise();
                event2.Raise();
            }
            TaskDialog.Show("OK", "事件发送结束，当前命令可以退出了");
            return Result.Succeeded;
        }
    }
}