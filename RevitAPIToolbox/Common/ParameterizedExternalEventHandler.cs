using Autodesk.Revit.UI;

namespace Techyard.Revit.Common
{
    public abstract class ParameterizedExternalEventHandler<T> : IExternalEventHandler
    {
        public T EventParameter { get; internal set; }

        public abstract void Execute(UIApplication app);

        public abstract string GetName();
    }
}