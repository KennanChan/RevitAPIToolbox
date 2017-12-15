using System.Collections.Generic;

namespace Techyard.Revit.Wheel.ExternalEvent
{
    /// <summary>
    ///     <para>A simple rebuilt version of Autodesk.Revit.UI.ExternalEvent class</para>
    ///     <para>Autodesk.Revit.UI.ExternalEvent 的简易轮子，外部事件触发器</para>
    /// </summary>
    public class ExternalEventWheel
    {
        /// <summary>
        ///     <para>Private constructor</para>
        ///     <para>构造函数私有化</para>
        /// </summary>
        /// <param name="handler">External event handler</param>
        private ExternalEventWheel(IExternalEventHandlerWheel handler)
        {
            Handler = handler;
        }

        /// <summary>
        ///     <para>A queue who holds all the raised external event handler to be executed sequentially</para>
        ///     <para>外部事件队列，用于记录被触发的外部事件处理程序</para>
        /// </summary>
        private static Queue<IExternalEventHandlerWheel> Events { get; } = new Queue<IExternalEventHandlerWheel>();

        /// <summary>
        ///     <para>The external event handler binding to current external event</para>
        ///     <para>绑定到当前外部事件的处理程序</para>
        /// </summary>
        private IExternalEventHandlerWheel Handler { get; }

        /// <summary>
        ///     <para>Create an external event to raise</para>
        ///     <para>创建一个外部事件</para>
        /// </summary>
        /// <param name="handler">External event handler</param>
        /// <returns>An external event through which the user can trigger the executing of the handler</returns>
        public static ExternalEventWheel Create(IExternalEventHandlerWheel handler)
        {
            //Some other operations may be done here(e.g parameter validating)
            return new ExternalEventWheel(handler);
        }

        /// <summary>
        ///     <para>Trigger the event so that the revit main process can comsume</para>
        ///     <para>触发当前外部事件，让Revit主线程执行事件处理程序</para>
        /// </summary>
        public void Raise()
        {
            lock (Events)
            {
                //Add current handler to the queue, waiting to be executed
                //将事件处理程序添加到队列，等待执行
                Events.Enqueue(Handler);
            }
        }

        /// <summary>
        ///     <para>Try to get a raised external event</para>
        ///     <para>尝试获取一个待执行的处理程序</para>
        /// </summary>
        /// <returns>The first external event handler</returns>
        internal static IExternalEventHandlerWheel TryGetEvent()
        {
            lock (Events)
            {
                //Get the very first external event handler for the queue
                //从队列获取一个事件处理程序
                return Events.Count > 0 ? Events.Dequeue() : null;
            }
        }
    }
}