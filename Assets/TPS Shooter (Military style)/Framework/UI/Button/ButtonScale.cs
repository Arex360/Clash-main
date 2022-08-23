using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace LightDev.UI
{
  [RequireComponent(typeof(Image))]
  public class ButtonScale : BaseButton
  {
    protected override void AnimatePress()
    {
      KillSequences();
      Sequence(target.transform.DOScale(0.8f, 0.1f));
    }

    protected override void AnimateUnpress()
    {
      KillSequences();
      Sequence(target.transform.DOScale(1f, 0.1f));
    }

    public override void ResetButton()
    {
      KillSequences();
      target.transform.localScale = Vector3.one;
    }
  }
}
