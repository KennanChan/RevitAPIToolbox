using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace Techyard.Revit.Wheel.ExternalEvent
{
    /// <summary>
    ///     <para>External application demenstrating how revit handles external events implemented by user</para>
    ///     <para>一个模拟Revit执行用户定义的外部事件(IExternalEventHandler)的Demo</para>
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            ExternalEventWheel.Initialize(application);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ExternalEventWheel.Destroy();
            return Result.Succeeded;
        }
    }
}
