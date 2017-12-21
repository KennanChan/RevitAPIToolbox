using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace Techyard.Revit.Common
{
    public abstract class ParameterizedExternalEventHandler<T> : IExternalEventHandler
    {
        private static Queue<T> Parameters { get; } = new Queue<T>();

        public T EventParameter
        {
            get
            {
                lock (Parameters)
                {
                    return Parameters.Dequeue();
                }
            }
            set
            {
                lock (Parameters)
                {
                    Parameters.Enqueue(value);
                }
            }
        }

        public void Execute(UIApplication app)
        {
            var parameter = EventParameter;
            Execute(app, parameter);
        }

        public abstract void Execute(UIApplication app, T parameter);

        public abstract string GetName();
    }
}