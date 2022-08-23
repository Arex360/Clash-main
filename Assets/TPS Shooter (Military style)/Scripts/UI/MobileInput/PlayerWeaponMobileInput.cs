using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PlayerWeaponMobileInput : CanvasElement
  {
    public override void Subscribe()
    {
      base.Subscribe();

      Events.SceneLoaded += NeedShow;
      Events.GameResumed += NeedShow;
      Events.PlayerGetOutVehicle += NeedShow;
      Events.PlayerShowWeapon += NeedShow;
      Events.PlayerDropWeapon += OnPlayerDropWeapon;
      Events.PlayerHideWeapon += Hide;
      Events.GamePaused += Hide;
      Events.GameFinished += Hide;
      Events.PlayerGetInVehicle += Hide;
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.SceneLoaded -= NeedShow;
      Events.GameResumed -= NeedShow;
      Events.PlayerGetOutVehicle -= NeedShow;
      Events.PlayerShowWeapon -= NeedShow;
      Events.PlayerDropWeapon -= OnPlayerDropWeapon;
      Events.PlayerHideWeapon -= Hide;
      Events.GamePaused -= Hide;
      Events.GameFinished -= Hide;
      Events.PlayerGetInVehicle -= Hide;
    }

    protected virtual void NeedShow()
    {
      if (PlayerBehaviour.GetInstance().IsDrivingVehicle) return;
      if (PlayerBehaviour.GetInstance().IsUnarmedMode) return;

      Show();
    }

    protected virtual void OnPlayerDropWeapon(PlayerWeapon weapon)
    {
      Hide();
    }

    public void OnFireStay() { Events.FireRequested.Call(); }
    public void OnReloadClick() { Events.ReloadRequested.Call(); }
    public void OnAimActivateClick() { Events.AimActivateRequested.Call(); }
  }
}
