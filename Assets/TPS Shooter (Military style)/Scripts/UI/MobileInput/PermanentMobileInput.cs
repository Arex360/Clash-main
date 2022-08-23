using UnityEngine;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PermanentMobileInput : CanvasElement
  {
    [Header("References")]
    public Touchpad[] touchpads;

    public override void Subscribe()
    {
      base.Subscribe();

      Events.SceneLoaded += Show;
      Events.GameResumed += Show;
      Events.GamePaused += Hide;
      Events.GameFinished += Hide;
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.SceneLoaded -= Show;
      Events.GameResumed -= Show;
      Events.GamePaused -= Hide;
      Events.GameFinished -= Hide;
    }

    private void Update()
    {
      foreach (Touchpad t in touchpads)
      {
        InputController.VerticalRotation = t.IsPressed ? t.VerticalValue : 0;
        InputController.HorizontalRotation = t.IsPressed ? t.HorizontalValue : 0;
        
        if (t.IsPressed) break;
      }
    }

    public void OnPauseGameClick() { Events.GamePauseRequested.Call(); }
  }
}
