using System.Collections;
using System.Collections.Concurrent;

public class Bus
{
    private ConcurrentDictionary<Type, IList> subscribers;

    public Bus()
    {
        subscribers = new ConcurrentDictionary<Type, IList>();
    }

    public void Publish<T>(T messagePayload)
    {
        Type messageType = typeof(T);

        CreateTopicIfNotExists<T>(messagePayload);

        foreach (var subscriber in subscribers[messageType].Cast<Action<T>>())
        {
            Task.Run(() => subscriber.Invoke(messagePayload));
        }
    }

    public void Subscribe<T>(Action<T> subscriber)
    {
        Type messageType = typeof(T);

        if(!subscribers.ContainsKey(messageType))
        {
            subscribers[messageType] = new List<Action<T>>();
        }

        subscribers[messageType].Add(subscriber);
    }

    public void Unsuscribe<T>(Action<T> subscriber)
    {
        Type messageType = typeof(T);

        if(!subscribers.ContainsKey(messageType))
        {
            return;
        }

        subscribers[messageType].Remove(subscriber);
    }

    private void CreateTopicIfNotExists<T>(T message)
    {
        Type messageType = typeof(T);

        if(subscribers.ContainsKey(messageType))
        {
            return;
        }

        subscribers[messageType] = new List<Action<T>>();
    }

    public void Dispose(string id)
    {

    }

    public IBusSubscription Sub()
    {
        return new BusSubscription(this);
    }

    private class BusSubscription : IBusSubscription
    {
        private readonly Bus bus;
        private readonly string id;

        public BusSubscription(Bus bus)
        {
            this.bus = bus;
            id = Guid.NewGuid().ToString();
        }

        public void Dispose()
        {
            bus.Dispose(id);
        }
    }
}

public interface IBusSubscription
{
    void Dispose();
}