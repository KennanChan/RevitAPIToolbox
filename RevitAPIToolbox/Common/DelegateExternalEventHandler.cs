using System;
using Autodesk.Revit.UI;

namespace Techyard.Revit.Common
{
    public class DelegateExternalEventHandler<T> : IExternalEventHandler
    {
        public DelegateExternalEventHandler(T parameter, Action<UIApplication, T> handler)
            : this(parameter, handler, Guid.NewGuid().ToString())
        {

        }

        public DelegateExternalEventHandler(T parameter, Action<UIApplication, T> handler, string name)
        {
            Parameter = parameter;
            Handler = handler;
            Name = name;
        }

        private T Parameter { get; }

        private Action<UIApplication, T> Handler { get; }

        private string Name { get; }

        public void Execute(UIApplication app)
        {
            Handler?.Invoke(app, Parameter);
        }

        public string GetName()
        {
            return Name;
        }
    }
}
