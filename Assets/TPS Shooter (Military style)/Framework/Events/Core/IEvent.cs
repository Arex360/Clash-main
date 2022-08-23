using System;

namespace LightDev
{
  public abstract class IEvent
  {
    protected readonly string name;

    protected IEvent(string name)
    {
      this.name = name;
    }

    protected void ThrowSubscribeException()
    {
      UnityEngine.Debug.LogException(new Exception("Event duplicate subscription detected: " + name));
    }
  }
}
