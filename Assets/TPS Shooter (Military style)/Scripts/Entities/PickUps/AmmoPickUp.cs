using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LightDev;

namespace TPSShooter
{
  public class AmmoPickUp : PickUp
  {
    public WeaponTag weaponTag;
    public int value;

    private static readonly string[] Tags = new string[]
    {
      "Val",
      "MiniGun",
      "Ak",
      "MachineGun",
      "AkScope",
      "Aug",
      "M4"
    };

    protected override void OnPlayerCollision(PlayerBehaviour player)
    {
      PlayerWeapon weapon = null;
      if (weaponTag == WeaponTag.AnyWeapon)
      {
        if (player.CurrentWeaponBehaviour)
        {
          weapon = player.CurrentWeaponBehaviour;
        }
      }
      else if (IsPlayerHasWeapon(player, weaponTag))
      {
        weapon = FindPlayerWeapon(player, weaponTag);
      }

      if (weapon)
      {
        weapon.BulletsAmount += value;
        Events.PlayerPickUpAmmo.Call();
        Destroy(gameObject);
      }
    }

    private bool IsPlayerHasWeapon(PlayerBehaviour player, WeaponTag tag)
    {
      var weapons = player.weaponSettings.AllWeapons;
      foreach (var weapon in weapons)
      {
        if (weapon.CompareTag(Tags[(int)tag]))
          return true;
      }

      return false;
    }

    private PlayerWeapon FindPlayerWeapon(PlayerBehaviour player, WeaponTag tag)
    {
      var weapons = player.weaponSettings.AllWeapons;
      foreach (var weapon in weapons)
      {
        if (weapon.CompareTag(Tags[(int)tag]))
          return weapon;
      }

      return null;
    }
  }

  // Tags that used in Player weapons
  // This enum is made for your more convenient use
  // If you add new weapons, remeber to add tags to those game objects and in enum
  [System.Serializable]
  public enum WeaponTag
  {
    Val = 0,
    MiniGun = 1,
    Ak = 2,
    MachineGun = 3,
    AkScope = 4,
    Aug = 5,
    M4 = 6,
    AnyWeapon = -1
  }
}
