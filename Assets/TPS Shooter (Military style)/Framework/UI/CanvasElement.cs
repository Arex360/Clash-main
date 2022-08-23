using UnityEngine;

using LightDev.Core;

namespace LightDev.UI
{
  /// <summary>
  /// UI element that controlled by CanvasManager.
  ///
  /// GameObject would be deactived, you need to subscribe on Events to Show.
  /// </summary>
  public abstract class CanvasElement : Base
  {
    [Range(0, 10)]
    public float showTime;
    [Range(0, 10)]
    public float hideTime;

    private Coroutine showCoroutine;
    private Coroutine hideCoroutine;

    /// <summary>
    /// Called by CanvasManager in Awake method.
    ///
    /// Used for subscribing on events in order to know when to Show UI.
    /// </summary>
    public virtual void Subscribe()
    {
    }

    /// <summary>
    /// Called by CanvasManager in OnDestroy method.
    ///
    /// Unsubscribe from all events.
    /// </summary>
    public virtual void Unsubscribe()
    {
    }

    protected virtual void OnStartShowing()
    {
    }

    protected virtual void OnFinishShowing()
    {
    }

    protected virtual void OnStartHiding()
    {
    }

    protected virtual void OnFinishHiding()
    {
    }

    /// <summary>
    /// 1) Activates GameObject.
    /// 2) Calls OnStartShowing().
    /// 3) After showTime delay calls OnFinishShowing().
    /// </summary>
    protected void Show()
    {
      StopShowCoroutine();
      StopHideCoroutine();

      Activate();
      OnStartShowing();
      showCoroutine = DelayAction(showTime, OnFinishShowing);
    }

    /// <summary>
    /// 1) Activates GameObject.
    /// 2) Calls OnStartShowing() and then OnFinishShowing() without showTime delay.
    /// </summary>
    protected void InstantShow()
    {
      StopShowCoroutine();
      StopHideCoroutine();

      Activate();
      OnStartShowing();
      OnFinishShowing();
    }

    /// <summary>
    /// 1) Calls OnStartHiding().
    /// 2) After hideTime delay calls OnFinishShowing().
    /// 3) Deactivates GameObject.
    /// </summary>
    protected void Hide()
    {
      if (gameObject.activeSelf == false) return;

      StopShowCoroutine();
      StopHideCoroutine();

      OnStartHiding();
      hideCoroutine = DelayAction(hideTime, () =>
      {
        OnFinishHiding();
        Deactivate();
      });
    }

    /// <summary>
    /// 1) Calls OnStartHiding() and then without hideTime delay OnFinishShowing().
    /// 2) Deactivates GameObject.
    /// </summary>
    protected void InstantHide()
    {
      if (gameObject.activeSelf == false) return;

      StopShowCoroutine();
      StopHideCoroutine();

      OnStartHiding();
      OnFinishHiding();
      Deactivate();
    }

    private void StopShowCoroutine()
    {
      if (showCoroutine != null)
      {
        StopCoroutine(showCoroutine);
        showCoroutine = null;
      }
    }

    private void StopHideCoroutine()
    {
      if (hideCoroutine != null)
      {
        StopCoroutine(hideCoroutine);
        hideCoroutine = null;
      }
    }
  }
}
