using Autodesk.Revit.UI;

namespace Techyard.Revit.Wheel.ExternalEvent
{
    public class ExternalEventHandlerDemo : IExternalEventHandlerWheel
    {
        public ExternalEventHandlerDemo(string name)
        {
            Name = name;
        }

        private string Name { get; }
        private int Count { get; set; }

        public void Execute(UIApplication app)
        {
            if (null == app) return;
            TaskDialog.Show("外部事件", $"这是{Name}第{++Count}次被调用");
        }
    }
}