using TPSShooter;

namespace LightDev
{
  public partial class Events
  {
    public static Event PlayerAimActivated;
    public static Event PlayerAimDeactivated;

    public static Event PlayerStartGrenadeThrow;
    public static Event PlayerFinishGreandeThrow;

    public static Event<PlayerWeapon> PlayerPickedUpWeapon;
    public static Event<PlayerWeapon> PlayerDropWeapon;

    public static Event PlayerHideWeapon;
    public static Event PlayerShowWeapon;
    public static Event PlayerChangedWeapon;

    public static Event PlayerReloaded;

    public static Event PlayerPickUpAmmo;
    public static Event PlayerPickUpHealthPack;

    public static Event PlayerStand;
    public static Event PlayerCrouch;

    public static Event PlayerFire;

    public static Event PlayerDied;

    public static Event PlayerDetectVehicle;
    public static Event PlayerUndetectVehicle;

    public static Event PlayerGetInVehicle;
    public static Event PlayerGetOutVehicle;

    public static Event PlayerChangedHP;

    public static Event<EnemyBullet> PlayerBulletHit;
    public static Event<ZombieBehaviour> PlayerZombieHit;
  }
}
