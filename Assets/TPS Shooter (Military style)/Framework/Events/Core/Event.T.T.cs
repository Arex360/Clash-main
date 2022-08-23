using System;
using System.Collections.Generic;

namespace LightDev
{
  public sealed class Event<T1, T2> : IEvent
  {
    private readonly List<Action<T1, T2>> subscribers;

    public Event(string name) : base(name)
    {
      subscribers = new List<Action<T1, T2>>();
    }

    public void Call(T1 parameter1, T2 parameter2)
    {
      for (int i = subscribers.Count - 1; i >= 0; --i)
      {
        subscribers[i].Invoke(parameter1, parameter2);
      }
    }

    public static Event<T1, T2> operator +(Event<T1, T2> e, Action<T1, T2> subscriber)
    {
      e.DoubleSubscriptionCheck(subscriber);
      e.subscribers.Add(subscriber);

      return e;
    }

    public static Event<T1, T2> operator -(Event<T1, T2> e, Action<T1, T2> subscriber)
    {
      e.subscribers.Remove(subscriber);

      return e;
    }

    private void DoubleSubscriptionCheck(Action<T1, T2> subscriber)
    {
      if (subscribers.Contains(subscriber))
      {
        ThrowSubscribeException();
      }
    }
  }
}
