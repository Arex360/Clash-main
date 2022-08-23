using LightDev;

namespace TPSShooter
{
  public partial class PlayerBehaviour
  {
    public bool IsAiming { get; private set; }

    private void OnAimActivateRequested()
    {
      if (!IsAlive) return;
      if (CurrentWeaponBehaviour == null) return;
      if (CurrentWeaponBehaviour.ScopeSettings.IsAimingAvailable == false) return;
      if (IsReloading) return;
      if (IsDrivingVehicle) return;
      if (IsSwapingWeapon) return;

      if (IsAiming)
        DeactivateAiming();
      else
        ActivateAiming();
    }

    private void ActivateAiming()
    {
      IsAiming = true;
      CurrentWeaponBehaviour.ScopeActive();

      sounds.PlaySound(sounds.ZoomInSound);

      Events.PlayerAimActivated.Call();
    }

    private void DeactivateAiming()
    {
      IsAiming = false;
      CurrentWeaponBehaviour.ScopeDisactivate();

      sounds.PlaySound(sounds.ZoomOutSound);

      Events.PlayerAimDeactivated.Call();
    }
  }
}
