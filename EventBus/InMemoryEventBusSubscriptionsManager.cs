using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public bool IsEmpty => !_handlers.Keys.Any();

        public void Clear() => _handlers.Clear();
        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        public InMemoryEventBusSubscriptionsManager(Dictionary<string, List<SubscriptionInfo>> handlers,
            List<Type> eventTypes)
        {
            _handlers = handlers;
            _eventTypes = eventTypes;
        }

        public event EventHandler<string> OnEventRemoved;

        public void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            DoAddSubscription(typeof(TH), eventName, isDynamic: true);
        }

        public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName, isDynamic: false);

            if (!_eventTypes.Contains(typeof(TH)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'",
                    nameof(handlerType));
            }

            if (isDynamic)
            {
                _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            }
        }

        public void RemoveSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var key = GetEventKey<T>();
            var handlerToRemove = FindSubscriptionInfo<T, TH>();
            DoRemoveHandler(key, handlerToRemove);
        }

        public void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            var handlerToRemove = FindDynamicSubscriptionInfoToRemove<TH>(eventName);
            DoRemoveHandler(eventName, handlerToRemove);
        }

        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }

                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        private SubscriptionInfo FindSubscriptionInfo<T, TH>() where T: IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            return DoFindSubscriptionToRemove(GetEventKey<T>(), typeof(TH));
        }

        private SubscriptionInfo FindDynamicSubscriptionInfoToRemove<TH>(string eventName) where TH: IDynamicIntegrationEventHandler
        {
            return DoFindSubscriptionToRemove(eventName, typeof(TH));
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                return null;
            }

            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }

        public bool HasSubscriptionForEvent<T>() where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();
            return HasSubscriptionForEvent(eventName);
        }

        public bool HasSubscriptionForEvent(string eventName) => _handlers.ContainsKey(eventName) && _handlers[eventName] != null;
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();
            return GetHandlersForEvent(eventName);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];
        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}
