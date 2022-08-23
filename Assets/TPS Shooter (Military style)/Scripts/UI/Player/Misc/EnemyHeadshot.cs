using UnityEngine;

using LightDev;
using LightDev.UI;
using LightDev.Core;

using DG.Tweening;

namespace TPSShooter.UI
{
  public class EnemyHeadshot : CanvasElement
  {
    [Header("References")]
    public Base info;

    public override void Subscribe()
    {
      Events.SceneLoaded += Show;
      Events.EnemyHeadshot += OnHeadshot;
      Events.ZombieHeadshot += OnHeadshot;
    }

    public override void Unsubscribe()
    {
      Events.SceneLoaded -= Show;
      Events.EnemyHeadshot -= OnHeadshot;
      Events.ZombieHeadshot -= OnHeadshot;
    }

    protected override void OnStartShowing()
    {
      info.SetFade(0);
    }

    private void OnHeadshot()
    {
      info.KillSequences();
      info.SetFade(0);
      info.Sequence(
        info.Fade(1, 0.3f).SetEase(Ease.InSine),
        info.Delay(1.5f),
        info.Fade(0, 0.3f).SetEase(Ease.InSine)
      );
    }
  }
}
