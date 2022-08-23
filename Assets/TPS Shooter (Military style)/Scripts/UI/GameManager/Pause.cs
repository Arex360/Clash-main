using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class Pause : CanvasElement
  {
    public override void Subscribe()
    {
      base.Subscribe();

      Events.GamePaused += Show;
      Events.GameResumed += Hide;
      Events.GameLoadHomeScene += Hide;
      Events.GameReplay += Hide;
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.GamePaused -= Show;
      Events.GameResumed -= Hide;
      Events.GameLoadHomeScene -= Hide;
      Events.GameReplay -= Hide;
    }

    public void OnResume()
    {
      Events.GameResumeRequested.Call();
    }

    public void OnReplay()
    {
      Events.GameReplayRequested.Call();
    }

    public void OnLoadHome()
    {
      Events.GameLoadHomeSceneRequested.Call();
    }
  }
}
