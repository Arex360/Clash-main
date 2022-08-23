using UnityEngine;
using UnityEngine.UI;

using LightDev.Core;

using DG.Tweening;

namespace LightDev.UI
{
  [RequireComponent(typeof(Text))]
  public class BaseText : Base
  {
    protected Text textComponent;

    protected virtual void Awake()
    {
      textComponent = GetComponent<Text>();
    }

    public virtual Text GetTextComponent()
    {
      return textComponent;
    }

    public virtual void SetText(string text)
    {
      textComponent.text = text;
    }

    public virtual void SetText(int text)
    {
      SetText(text.ToString());
    }

    public virtual Tween TweenColor(Color to, float duration)
    {
      return textComponent.DOColor(to, duration);
    }

    public virtual Tween TweenFade(float to, float duration)
    {
      return textComponent.DOFade(to, duration);
    }

    public virtual Tween TweenText(string to, float duration)
    {
      return textComponent.DOText(to, duration);
    }
  }
}
