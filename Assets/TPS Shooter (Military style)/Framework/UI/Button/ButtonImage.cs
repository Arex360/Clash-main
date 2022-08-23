using UnityEngine;
using UnityEngine.UI;

namespace LightDev.UI
{
  [RequireComponent(typeof(Image))]
  public class ButtonImage : BaseButton
  {
    [Space]
    public Sprite normalImage;
    public Sprite pressedImage;

    protected override void Awake()
    {
      base.Awake();
      target.sprite = normalImage;
    }

    protected override void AnimatePress()
    {
      target.sprite = pressedImage;
    }

    protected override void AnimateUnpress()
    {
      target.sprite = normalImage;
    }

    public override void ResetButton()
    {
      target.sprite = normalImage;
    }
  }
}
