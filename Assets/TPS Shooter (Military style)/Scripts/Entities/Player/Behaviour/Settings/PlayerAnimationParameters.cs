namespace TPSShooter
{
  [System.Serializable]
  public class PlayerAnimationParameters
  {
    // movement
    public string verticalMovementFloat = "Forward";
    public string horizontalMovementFloat = "Strafe";
    // run
    public string runBool = "IsRun";
    // weapon mode
    public string weaponInt = "WeaponMode";
    public string changeWeaponTrigger = "ChangeWeapon";
    // aiming
    public string aimingBool = "IsAiming";
    // reloading
    public string reloadTrigger = "Reload";
    // jumping
    public string jumpBool = "IsJumping";
    public string groundedBool = "IsGrounded";
    // crouch
    public string crouchBool = "IsCrouch";
    // grenade
    public string standGrenadeTrigger = "StandGrenade";
    public string standThrowGrenadeTrigger = "ThrowGrenade";
    // car
    public string vehicleBool = "InVehicle";
    // death
    public string dieTrigger = "Died";
  }
}
