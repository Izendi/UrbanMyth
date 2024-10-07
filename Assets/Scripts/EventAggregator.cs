using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Contracts;
using Assets.Scripts.Events;

namespace Assets.Scripts
{
    public class EventAggregator
    {
        // Dictionary where each event type has a list of handlers (value)
        private readonly Dictionary<Type, List<object>> _eventHandlers = new Dictionary<Type, List<object>>();

        // Singleton instance
        public static EventAggregator Instance { get; } = new EventAggregator();

        public void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);
            if (!_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = new List<object>();
            }
            _eventHandlers[eventType].Add(handler);
        }

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType].Remove(handler);
            }
        }

        // Publish an event to all registered handlers
        public void Publish<TEvent>(TEvent ev) where TEvent : IEvent
        {
            Type eventType = typeof(TEvent);
            if (_eventHandlers.ContainsKey(eventType))
            {
                foreach (var handler in _eventHandlers[eventType])
                {
                    ((IEventHandler<TEvent>)handler).Handle(ev);
                }
            }
        }
    }
}