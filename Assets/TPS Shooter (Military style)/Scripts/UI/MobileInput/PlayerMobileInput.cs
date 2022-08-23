using UnityEngine;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PlayerMobileInput : CanvasElement
  {
    [Header("References")]
    public VirtualJoystick joystick;

    public override void Subscribe()
    {
      base.Subscribe();

      Events.SceneLoaded += Show;
      Events.GamePaused += Hide;
      Events.GameResumed += OnGameResumed;
      Events.GameFinished += Hide;
      Events.PlayerGetInVehicle += Hide;
      Events.PlayerGetOutVehicle += Show;
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.SceneLoaded -= Show;
      Events.GamePaused -= Hide;
      Events.GameResumed -= OnGameResumed;
      Events.GameFinished -= Hide;
      Events.PlayerGetInVehicle -= Hide;
      Events.PlayerGetOutVehicle -= Show;
    }

    protected virtual void OnGameResumed()
    {
      if (PlayerBehaviour.GetInstance().IsDrivingVehicle) return;

      Show();
    }

    protected virtual void Update()
    {
      InputController.HorizontalMovement = joystick.HorizontalValue;
      InputController.VerticalMovement = joystick.VerticalValue;
      InputController.IsRun = joystick.IsRun;
    }

    public void OnCrouchClick() { Events.CrouchRequested.Call(); }
    public void OnJumpClick() { Events.JumpRequested.Call(); }

    public void OnGrenadeThrowDown() { Events.GrenadeStartThrowRequest.Call(); }
    public void OnGrenadeThrowUp() { Events.GrenadeFinishThrowRequest.Call(); }

    public void OnWeaponChooseRequest() { Events.WeaponChooseStartRequest.Call(); }
  }
}
