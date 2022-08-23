using UnityEngine;

namespace TPSShooter
{
  [System.Serializable]
  public class PlayerWeaponSettings
  {
    public LayerMask shootingLayers;

    public PlayerWeapon CurrentWeapon;
    public PlayerWeapon[] AllWeapons;
  }
}
