using System;
using System.Collections.Generic;

namespace LightDev
{
  public sealed class Event<T> : IEvent
  {
    private readonly List<Action<T>> subscribers;

    public Event(string name) : base(name)
    {
      subscribers = new List<Action<T>>();
    }

    public void Call(T parameter)
    {
      for (int i = subscribers.Count - 1; i >= 0; --i)
      {
        subscribers[i].Invoke(parameter);
      }
    }

    public static Event<T> operator +(Event<T> e, Action<T> subscriber)
    {
      e.DoubleSubscriptionCheck(subscriber);
      e.subscribers.Add(subscriber);

      return e;
    }

    public static Event<T> operator -(Event<T> e, Action<T> subscriber)
    {
      e.subscribers.Remove(subscriber);

      return e;
    }

    private void DoubleSubscriptionCheck(Action<T> subscriber)
    {
      if (subscribers.Contains(subscriber))
      {
        ThrowSubscribeException();
      }
    }
  }
}