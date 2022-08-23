using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class Finish : CanvasElement
  {
    public override void Subscribe()
    {
      base.Subscribe();

      Events.GameFinished += Show;
      Events.GameLoadHomeScene += Hide;
      Events.GameReplay += Hide;
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.GameFinished -= Show;
      Events.GameLoadHomeScene -= Hide;
      Events.GameReplay -= Hide;
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
