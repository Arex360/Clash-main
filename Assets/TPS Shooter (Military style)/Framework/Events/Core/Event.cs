using System;
using System.Collections.Generic;

namespace LightDev
{
  public sealed class Event : IEvent
  {
    private readonly List<Action> subscribers;

    public Event(string name) : base(name)
    {
      subscribers = new List<Action>();
    }

    public void Call()
    {
      for(int i = subscribers.Count - 1; i >= 0; --i)
      {
        subscribers[i].Invoke();
      }
    }

    public static Event operator +(Event e, Action subscriber)
    {
      e.DoubleSubscriptionCheck(subscriber);
      e.subscribers.Add(subscriber);

      return e;
    }

    public static Event operator -(Event e, Action subscriber)
    {
      e.subscribers.Remove(subscriber);

      return e;
    }


    private void DoubleSubscriptionCheck(Action subscriber)
    {
      if (subscribers.Contains(subscriber))
      {
        ThrowSubscribeException();
      }
    }
  }
}
