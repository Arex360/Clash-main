using UnityEngine;

namespace TPSShooter
{
  public partial class PlayerBehaviour
  {
    private void UpdateSpineIK()
    {
      bool IsAimingIK = NeedToUseSpineIK();
      _animator.SetBool(animationsParameters.aimingBool, IsAimingIK);

      if (IsAimingIK)
        PositionSpine();
    }

    private void UpdateLeftHandIK()
    {
      if (IsAlive == false) return;

      if (CurrentWeaponBehaviour &&
        !IsReloading &&
        !IsThrowingGrenade &&
        !IsUnarmedMode &&
        !IsSwapingWeapon &&
        (!IsAiming || IsAiming && !CurrentWeaponBehaviour.ScopeSettings.IsFPS))
      {
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, CurrentWeaponBehaviour.LeftHandIk.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, CurrentWeaponBehaviour.LeftHandIk.rotation);
      }
      else
      {
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
      }
    }

    private bool NeedToUseSpineIK()
    {
      if (IsAlive == false) return false;
      if (IsRunning) return false;
      if (IsReloading) return false;
      if (IsThrowingGrenade) return false;
      if (IsUnarmedMode) return false;
      if (IsDrivingVehicle) return false;

      return true;
    }

    private void PositionSpine()
    {
      IkSettings.Spine.LookAt(IkSettings.LookAt);

      if (IsAiming)
        IkSettings.Spine.Rotate(CurrentWeaponBehaviour.ScopeSettings.PlayerSpineRotation);
      else
        IkSettings.Spine.Rotate(IkSettings.SpineRotation);
    }
  }
}
