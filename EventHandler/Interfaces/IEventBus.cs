using System;

namespace EventHandler.Interfaces
{
    public interface IEventBus
    {
        /// <summary>
        /// Subscribe the action to a specified event 
        /// </summary>
        /// <param name="action">The action which want to be invoked when the event is published</param>
        /// <typeparam name="TEvent">The type of an event which is derived from <see cref="IEventBase"/></typeparam>
        void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IEventBase;
        /// <summary>
        /// Subscribe the action in run time to a specified event
        /// </summary>
        /// <param name="type">The type of an event which is derived from <see cref="IEventBase"/></param>
        /// <param name="action">The action which want to be invoked when the event is published</param>
        /// <exception cref="ArgumentException">Throw if the type is not derived from IEventBase</exception>
        void Subscribe(Type type, Action<IEventBase> action);
        /// <summary>
        /// Unsubscribe the action to a specified event 
        /// </summary>
        /// <param name="action">The action which want to remove from a specified event</param>
        /// <typeparam name="TEvent">The type of an event which is derived from <see cref="IEventBase"/></typeparam>
        void Unsubscribe<TEvent>(Action<TEvent> action) where TEvent : IEventBase;
        /// <summary>
        /// Unsubscribe the action in run time to a specified event
        /// </summary>
        /// <param name="type">The type of an event which is derived from <see cref="IEventBase"/></param>
        /// <param name="action">The action which want to remove from a specified event</param>
        /// <exception cref="ArgumentException">Throw if the type is not derived from IEventBase</exception>
        /// <exception cref="ArgumentNullException">Throw if the action is null</exception>
        void Unsubscribe(Type type, Action<IEventBase> action);
        /// <summary>
        /// Publish the event and invoke actions which are subscribed to this
        /// </summary>
        /// <param name="eventBase">The Event to publish</param>
        void Publish(IEventBase eventBase);
    }
}