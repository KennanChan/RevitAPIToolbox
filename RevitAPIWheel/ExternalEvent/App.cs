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
            application.Idling += Application_Idling;
            return Result.Succeeded;
        }

        /// <summary>
        ///     <para>An event that revit signals repeatedly at idle time</para>
        ///     <para>Revit在空闲时间反复调用的回调方法，当做 white(true){} 无限循环使用</para>
        /// </summary>
        /// <param name="sender">The revit UI level application</param>
        /// <param name="e">Arguments to config the event</param>
        private static void Application_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            //Internal method for the main process to get a raised external event handler to execute
            var handler = ExternalEventWheel.TryGetEvent();
            if (null == handler) return;
            var app = sender as UIApplication;
            if (null == app) return;
            handler.Execute(app);
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            application.Idling -= Application_Idling;
            return Result.Succeeded;
        }
    }
}
