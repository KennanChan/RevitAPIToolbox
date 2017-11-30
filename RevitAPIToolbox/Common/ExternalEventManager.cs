using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace Techyard.Revit.Common
{
    public class ExternalEventManager
    {
        private ExternalEventManager()
        {

        }

        private IDictionary<Type, ExternalEventPair> Events { get; } =
            new Dictionary<Type, ExternalEventPair>();

        public static ExternalEventManager Manager { get; } = new ExternalEventManager();

        public bool Register<T>(T handler) where T : IExternalEventHandler
        {
            var type = typeof(T);
            lock (Events)
            {
                if (Events.ContainsKey(type)) return false;
                Events.Add(type, new ExternalEventPair(ExternalEvent.Create(handler), handler));
                return true;
            }
        }

        public bool Raise<T>() where T : IExternalEventHandler
        {
            var type = typeof(T);
            return Events.ContainsKey(type) && PrivateRaise(type);
        }

        public bool Raise<THandler, TParameter>(TParameter parameter)
            where THandler : ParameterizedExternalEventHandler<TParameter>
        {
            var type = typeof(THandler);
            if (!Events.ContainsKey(type)) return false;
            if (!(Events[type].EventHandler is ParameterizedExternalEventHandler<TParameter> handler)) return false;
            handler.EventParameter = parameter;
            return PrivateRaise(type);
        }

        private bool PrivateRaise(Type type)
        {
            var request = Events[type].EventTrigger.Raise();
            switch (request)
            {
                case ExternalEventRequest.Pending:
                case ExternalEventRequest.Accepted:
                    return true;
                case ExternalEventRequest.Denied:
                case ExternalEventRequest.TimedOut:
                    return false;
                default:
                    return false;
            }
        }

        private struct ExternalEventPair
        {
            internal ExternalEventPair(ExternalEvent trigger, IExternalEventHandler handler)
            {
                EventTrigger = trigger;
                EventHandler = handler;
            }

            internal ExternalEvent EventTrigger { get; }
            internal IExternalEventHandler EventHandler { get; }
        }
    }
}
