using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class VehicleMobileInput : CanvasElement
  {
    public override void Subscribe()
    {
      base.Subscribe();

      Events.PlayerGetInVehicle += Show;
      Events.PlayerGetOutVehicle += Hide;
      Events.GamePaused += Hide;
      Events.GameResumed += OnGameResumed;
      Events.GameFinished += Hide;
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.PlayerGetInVehicle -= Show;
      Events.PlayerGetOutVehicle -= Hide;
      Events.GamePaused -= Hide;
      Events.GameResumed -= OnGameResumed;
      Events.GameFinished -= Hide;
    }

    protected virtual void OnGameResumed()
    {
      if (PlayerBehaviour.GetInstance().IsDrivingVehicle)
      {
        Show();
      }
    }

    public void OnVehicleFwdDown() { InputController.VerticalMovement = 1; }
    public void OnVehicleFwdUp() { InputController.VerticalMovement = 0; }
    public void OnVehicleBwdDown() { InputController.VerticalMovement = -1; }
    public void OnVehicleBwdUp() { InputController.VerticalMovement = 0; }
    public void OnVehicleRightDown() { InputController.HorizontalMovement = 1; }
    public void OnVehicleRightUp() { InputController.HorizontalMovement = 0; }
    public void OnVehicleLeftDown() { InputController.HorizontalMovement = -1; }
    public void OnVehicleLeftUp() { InputController.HorizontalMovement = 0; }
    public void OnBrakeDown() { InputController.IsBrakePressed = true; }
    public void OnBrakeUp() { InputController.IsBrakePressed = false; }
  }
}
