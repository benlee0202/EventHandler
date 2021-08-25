using System;
using EventHandler.Interfaces;

namespace EventHandler
{
    public class Subscription<TEvent> : ISubscription where TEvent : IEventBase
    {
        private readonly Action<IEventBase> _envelopedAction;
        public Delegate SubscriptionToken { get; }
        
        public Subscription(Action<TEvent> action)
        {
            _envelopedAction = eventBase => action((TEvent) eventBase);
            SubscriptionToken = action;
        }

        public Subscription(Action<IEventBase> action)
        {
            _envelopedAction = action;
            SubscriptionToken = action;
        }
        
        public void Execute(IEventBase eventBase)
        {
            _envelopedAction.Invoke(eventBase);
        }
    }
}