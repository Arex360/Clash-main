using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using LightDev.Core;

using DG.Tweening;

namespace LightDev.UI
{
  [RequireComponent(typeof(Image))]
  public abstract class BaseButton : Base, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
  {
    public Image target;
    public UnityEvent onClick;

    private Image image;

    public bool IsInteractable() { return image.raycastTarget; }
    public void SetIsInteractable(bool isInteractable) { image.raycastTarget = isInteractable; }

    protected virtual void Awake()
    {
      image = GetComponent<Image>();
      image.raycastTarget = true;
      target.raycastTarget = false;
    }

    protected virtual void OnEnable()
    {
      ResetButton();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      onClick?.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      AnimatePress();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
      AnimateUnpress();
    }

    public virtual void AnimateFadeShow(float delay = 0, float showTime = 0.6f)
    {
      ShowAnimationUtil(
        () => target.color = new Color(target.color.r, target.color.g, target.color.b, 0),
        target.DOFade(1, showTime).SetEase(Ease.InSine),
        delay
      );
    }

    public virtual void AnimateScaleShow(float delay = 0, float showTime = 0.3f)
    {
      ShowAnimationUtil(
        () => target.transform.localScale = Vector3.zero,
        target.transform.DOScale(1, showTime).SetEase(Ease.OutBack),
        delay
      );
    }

    private void ShowAnimationUtil(System.Action preAction, Tween tween, float delay)
    {
      KillSequences();
      SetIsInteractable(false);
      preAction?.Invoke();
      Sequence(
        Delay(delay),
        tween,
        OnFinish(() => SetIsInteractable(true))
      );
    }

    protected abstract void AnimatePress();
    protected abstract void AnimateUnpress();
    public abstract void ResetButton();
  }
}
