using System;

namespace Demo.Queue
{
    public interface IQueue
    {
        void PublishAsync(object data, string eventName);
        string SubscribeAsync(string eventName, Func<object, object> func);
        void UnsubscribeAsync(string id);
    }
}