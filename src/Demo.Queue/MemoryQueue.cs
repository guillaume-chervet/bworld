using System;
using System.Collections.Generic;

namespace Demo.Queue
{
    public class MemoryQueue : IQueue
    {
        private readonly IDictionary<string, IDictionary<string, Func<object, object>>> _subscribers = new  Dictionary<string, IDictionary<string, Func<object, object>>>();

        public void PublishAsync(object data, string eventName)
        {
            var list = _subscribers[eventName];
            foreach (var item in list)
            {
                item.Value(data);
            }
        }

        public string SubscribeAsync(string eventName, Func<object, object> func)
        {
            var id = Guid.NewGuid().ToString();
            if (!_subscribers.ContainsKey(eventName))
            {
                _subscribers.Add(eventName, new Dictionary<string, Func<object, object>>());
            }
            _subscribers[eventName].Add(id, func);
            return id;
        }

        public void UnsubscribeAsync(string id)
        {
            foreach (var subscriber in _subscribers)
            {
                var subscriberValue = subscriber.Value;
                if (subscriberValue.ContainsKey(id))
                {
                    subscriberValue.Remove(id);
                }
            }
        }

    }
}
