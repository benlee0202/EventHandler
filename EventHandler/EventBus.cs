using System;
using System.Collections.Generic;
using System.Linq;
using EventHandler.Interfaces;

namespace EventHandler
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<ISubscription>> _subscriptionsMap =
            new Dictionary<Type, List<ISubscription>>();

        private readonly Dictionary<Delegate, List<Type>> _typeMap = new Dictionary<Delegate, List<Type>>();

        public void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IEventBase
        {
            if (_subscriptionsMap.TryGetValue(typeof(TEvent), out var subscriptions))
            {
                subscriptions.Add(new Subscription<TEvent>(action));
            }
            else
            {
                _subscriptionsMap.Add(typeof(TEvent), new List<ISubscription>());
                _subscriptionsMap[typeof(TEvent)].Add(new Subscription<TEvent>(action));
            }
            
            if (_typeMap.TryGetValue(action, out var types))
            {
                types.Add(typeof(TEvent));
            }
            else
            {
                _typeMap.Add(action, new List<Type>());
                _typeMap[action].Add(typeof(TEvent));
            }
        }
        
        public void Subscribe(Type type, Action<IEventBase> action)
        {
            if (!type.GetInterfaces().Contains(typeof(IEventBase)))
            {
                throw new ArgumentException($"{type} should implement IEventBase");
            }
            
            if (_subscriptionsMap.TryGetValue(type, out var subscriptions))
            {
                subscriptions.Add(new Subscription<IEventBase>(action));
            }
            else
            {
                _subscriptionsMap.Add(type, new List<ISubscription>());
                _subscriptionsMap[type].Add(new Subscription<IEventBase>(action));
            }
            
            if (_typeMap.TryGetValue(action, out var types))
            {
                types.Add(type);
            }
            else
            {
                _typeMap.Add(action, new List<Type>());
                _typeMap[action].Add(type);
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> action) where TEvent : IEventBase
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            
            var types = _typeMap[action];
            if (!types.Contains(typeof(TEvent))) return;
            
            ISubscription subscriptionToRemove = null;
            
            foreach (var subscription in _subscriptionsMap[typeof(TEvent)]
                .Where(subscription => subscription.SubscriptionToken == (Delegate) action))
            {
                subscriptionToRemove = subscription;
            }

            if (subscriptionToRemove == null) return;
            
            _subscriptionsMap[typeof(TEvent)].Remove(subscriptionToRemove);
            _typeMap[action].Remove(typeof(TEvent));
            if (_typeMap[action].Count == 0) _typeMap.Remove(action);
        }
        
        public void Unsubscribe(Type type, Action<IEventBase> action)
        {
            if (!type.GetInterfaces().Contains(typeof(IEventBase)))
            {
                throw new ArgumentException($"{type} should implement IEventBase");
            }
            
            if (action == null) throw new ArgumentNullException(nameof(action));
            
            var types = _typeMap[action];
            if (!types.Contains(type)) return;
            
            ISubscription subscriptionToRemove = null;
            
            foreach (var subscription in _subscriptionsMap[type]
                .Where(subscription => subscription.SubscriptionToken == (Delegate) action))
            {
                subscriptionToRemove = subscription;
            }

            if (subscriptionToRemove == null) return;
            
            _subscriptionsMap[type].Remove(subscriptionToRemove);
            _typeMap[action].Remove(type);
            if (_typeMap[action].Count == 0) _typeMap.Remove(action);
        }

        public void Publish(IEventBase eventBase)
        {
            if (!_subscriptionsMap.TryGetValue(eventBase.GetType(), out var subscriptions)) return;
            foreach (var subscription in subscriptions)
            {
                subscription.Execute(eventBase);
            }
        }
    }
}
