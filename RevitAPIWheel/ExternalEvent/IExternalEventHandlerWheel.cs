using Autodesk.Revit.UI;

namespace Techyard.Revit.Wheel.ExternalEvent
{
    /// <summary>
    ///     <para>A simple rebuilt version of Autodesk.Revit.UI.IExternalEventHandler interface</para>
    ///     <para>Autodesk.Revit.UI.IExternalEventHandler 的简易轮子，外部事件处理程序</para>
    /// </summary>
    public interface IExternalEventHandlerWheel
    {
        void Execute(UIApplication app);
    }
}