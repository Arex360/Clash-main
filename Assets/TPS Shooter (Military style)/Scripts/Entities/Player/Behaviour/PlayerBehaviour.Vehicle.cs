using UnityEngine;

namespace TPSShooter
{
  public partial class PlayerBehaviour
  {
    public bool IsDrivingVehicle { get; set; }
    public Vehicle DrivingVehicle { get; set; }

    private void OnPlayerGetInVehicle()
    {
      _animator.SetBool(animationsParameters.vehicleBool, true);

      if (IsAiming)
        DeactivateAiming();

      if (weaponSettings.CurrentWeapon)
        weaponSettings.CurrentWeapon.gameObject.SetActive(false);

      // disables detection collisions in order to player's collider does not interact with vehicle collider
      _characterController.detectCollisions = false;
    }

    private void OnPlayerGetOutVehicle()
    {
      _animator.SetBool(animationsParameters.vehicleBool, false);

      if (weaponSettings.CurrentWeapon)
        weaponSettings.CurrentWeapon.gameObject.SetActive(true);

      // Activate detection collision in this period of time in order to player's collider doesn't interact with vehicle's collider
      DelayAction(Time.fixedDeltaTime * 2, () => _characterController.detectCollisions = true, false);
    }
  }
}
