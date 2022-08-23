using UnityEngine;
using UnityEngine.UI;

namespace LightDev.UI
{
  [RequireComponent(typeof(Image))]
  public class ButtonColor : BaseButton
  {
    [Space]
    public Color pressedColor = new Color(200 / 255f, 200 / 255f, 200 / 255f, 255 / 255f);

    protected Color normalColor;

    protected override void Awake()
    {
      base.Awake();

      normalColor = GetColor();
    }

    protected override void AnimatePress()
    {
      KillSequences();
      Sequence(Color(pressedColor, 0.2f));
    }

    protected override void AnimateUnpress()
    {
      KillSequences();
      Sequence(Color(normalColor, 0.2f));
    }

    public override void ResetButton()
    {
      KillSequences();
      target.color = normalColor;
    }
  }
}
