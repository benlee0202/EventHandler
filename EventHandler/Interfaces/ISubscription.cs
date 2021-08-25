using System;

namespace EventHandler.Interfaces
{
    public interface ISubscription
    {
        Delegate SubscriptionToken { get; }
        void Execute(IEventBase eventBase);
    }
}