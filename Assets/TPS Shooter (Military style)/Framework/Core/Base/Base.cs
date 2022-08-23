using System;
using System.Collections;

using UnityEngine;

namespace LightDev.Core
{
  /// <summary>
  /// Base class in LightDev framework that derived from MonoBehaviour.
  /// </summary>
  public partial class Base : MonoBehaviour
  {
    /// <summary>
    /// Activates GameObject
    /// </summary>
    public void Activate()
    {
      this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactivates GameObject
    /// </summary>
    public void Deactivate()
    {
      this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Invoke action after delay.
    ///
    /// Returns Coroutine, so you can stop it if it is needed.
    /// </summary>
    public Coroutine DelayAction(float delay, Action action, bool timeIndependent = true)
    {
      return StartCoroutine(DelayCoroutine(delay, action, timeIndependent));
    }

    private IEnumerator DelayCoroutine(float delay, Action action, bool timeIndependent = true)
    {
      float time = delay;
      while (time >= 0)
      {
        time -= timeIndependent ? Time.unscaledDeltaTime : Time.deltaTime;
        yield return null;
      }

      action.Invoke();
    }
  }
}
