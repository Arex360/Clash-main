using System.Collections;
using UnityEngine;
using LightDev;

namespace TPSShooter
{
  public partial class PlayerBehaviour
  {
    public bool IsFire { get; private set; }
    public bool IsReloading { get; private set; }
    public bool IsSwapingWeapon { get; private set; }
    public bool IsUnarmedMode { get; private set; }

    public Vector3 FirePoint { get; private set; }
    public Transform FireHitObject { get; private set; }

    public PlayerWeapon CurrentWeaponBehaviour { get { return weaponSettings.CurrentWeapon; } set { weaponSettings.CurrentWeapon = value; } }
    public int CurrentWeaponIndex { get; private set; } = -1;

    private Coroutine fireCoroutine;

    private int MaxWeaponCount = 8;

    private void ValidateWeapons()
    {
      if (weaponSettings.AllWeapons.Length == MaxWeaponCount) return;

      PlayerWeapon[] weapons = new PlayerWeapon[MaxWeaponCount];
      for (int i = 0; i < MaxWeaponCount && i < weaponSettings.AllWeapons.Length; i++)
      {
        weapons[i] = weaponSettings.AllWeapons[i];
      }
      weaponSettings.AllWeapons = weapons;
    }

    private void InitializeWeapon()
    {
      int weaponIndex = -1;
      foreach(var weapon in weaponSettings.AllWeapons)
      {
        weapon?.Deactivate();
      }
      for (int i = 0; i < weaponSettings.AllWeapons.Length && CurrentWeaponBehaviour != null; i++)
      {
        var weapon = weaponSettings.AllWeapons[i];
        if (weapon != null && weapon.CompareTag(CurrentWeaponBehaviour.tag))
        {
          weaponIndex = i;
          break;
        }
      }

      if (weaponIndex == -1)
      {
        HideCurrentWeapon();
      }
      else
      {
        SetCurrentWeapon(weaponIndex);
      }
    }

    private void OnFireRequested()
    {
      if (IsAlive == false) return;

      if (fireCoroutine != null)
      {
        StopCoroutine(fireCoroutine);
      }
      fireCoroutine = StartCoroutine(UpdateIsFire());

      if (CurrentWeaponBehaviour == null) return;
      if (IsThrowingGrenade) return;
      if (CurrentWeaponBehaviour.CanShoot == false) return;
      if (IsDrivingVehicle) return;

      Fire();
    }

    private void OnReloadRequested()
    {
      if (!IsAlive) return;
      if (CurrentWeaponBehaviour == null) return;
      if (IsReloading) return;
      if (IsThrowingGrenade) return;
      if (IsDrivingVehicle) return;
      if (!CurrentWeaponBehaviour.CanReload) return;

      Reload();
    }

    private void OnSwapWeaponRequested(int weaponIndex)
    {
      if (!IsAlive) return;
      if (IsSwapingWeapon) return;
      if (IsReloading) return;
      if (IsThrowingGrenade) return;
      if (IsDrivingVehicle) return;

      SetCurrentWeapon(weaponIndex);
    }

    private void OnDropWeaponRequested(int weaponIndex)
    {
      if (!IsAlive) return;
      if (IsUnarmedMode) return;
      if (IsDrivingVehicle) return;

      DropWeapon(weaponIndex);
    }

    private void Fire()
    {
      CurrentWeaponBehaviour.Fire(FirePoint);
      Events.PlayerFire.Call();
      if (CurrentWeaponBehaviour.BulletsInMag == 0)
      {
        Events.ReloadRequested.Call();
      }
    }

    private IEnumerator UpdateIsFire()
    {
      IsFire = true;
      yield return new WaitForEndOfFrame();
      yield return new WaitForEndOfFrame();
      IsFire = false;
    }

    private void UpdateFirePoint()
    {
      Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, Mathf.Infinity, weaponSettings.shootingLayers))
      {
        FirePoint = hit.point;
        FireHitObject = hit.collider.transform;
      }
      else
      {
        FirePoint = IkSettings.LookAt.position;
        FireHitObject = null;
      }
    }

    private void Reload()
    {
      IsReloading = true;
      _animator.SetTrigger(animationsParameters.reloadTrigger);

      CurrentWeaponBehaviour.Reload();

      if (IsAiming)
        DeactivateAiming();
    }

    // AnimationEvent
    // Called when the player's animation Reloading is finished
    private void FinishedReloading()
    {
      CurrentWeaponBehaviour.ReloadFinished();
      IsReloading = false;
      Events.PlayerReloaded.Call();
    }

    // Returns true if player has empty slot
    public bool CanPickUpWeapon()
    {
      for (int i = 0; i < weaponSettings.AllWeapons.Length; ++i)
      {
        if (weaponSettings.AllWeapons[i] == null)
        {
          return true;
        }
      }

      return false;
    }

    // Picks up weapon to first empty slot
    public void PickUpWeapon(PlayerWeapon newWeapon)
    {
      int insertIndex = 0;

      for (int i = 0; i < weaponSettings.AllWeapons.Length; i++)
      {
        if (weaponSettings.AllWeapons[i] == null)
        {
          weaponSettings.AllWeapons[i] = newWeapon;
          insertIndex = i;
          break;
        }
      }

      SetCurrentWeapon(insertIndex);
      Events.PlayerPickedUpWeapon.Call(newWeapon);
    }

    private void DropWeapon(int dropWeaponIndex)
    {
      if (dropWeaponIndex < 0) return;
      if (dropWeaponIndex >= weaponSettings.AllWeapons.Length) return;
      if (weaponSettings.AllWeapons[dropWeaponIndex] == null) return;
      PlayerWeaponPickUp weaponPickUp = weaponSettings.AllWeapons[dropWeaponIndex].GetComponent<PlayerWeaponPickUp>();
      if (weaponPickUp == null)
        return;

      if (IsAiming)
        DeactivateAiming();

      sounds.PlaySound(sounds.ChangeWeaponSound);

      if (dropWeaponIndex == CurrentWeaponIndex)
      {
        IsUnarmedMode = true;

        _animator.SetInteger(animationsParameters.weaponInt, -1);

        CurrentWeaponBehaviour = null;
        CurrentWeaponIndex = -1;
      }

      weaponPickUp.gameObject.SetActive(true);
      weaponPickUp.DropWeapon();
      weaponSettings.AllWeapons[dropWeaponIndex] = null;

      Events.PlayerDropWeapon.Call(weaponSettings.AllWeapons[dropWeaponIndex]);
    }

    private void SetCurrentWeapon(int weaponIndex)
    {
      if (weaponIndex >= 0 && weaponIndex < weaponSettings.AllWeapons.Length && weaponSettings.AllWeapons[weaponIndex] != null)
      {
        if (weaponIndex == CurrentWeaponIndex)
        {
          HideCurrentWeapon();
        }
        else
        {
          SetCurrentWeaponUtiliy(weaponIndex);
        }
      }
    }

    private void SetCurrentWeaponUtiliy(int weaponIndex)
    {
      ChangeWeaponUtility(false);
      CurrentWeaponIndex = weaponIndex;
    }

    private void HideCurrentWeapon()
    {
      ChangeWeaponUtility(true);
    }

    private void ChangeWeaponUtility(bool hideWeapon)
    {
      if (IsAiming)
        DeactivateAiming();

      sounds.PlaySound(sounds.ChangeWeaponSound);
      _animator.SetTrigger(animationsParameters.changeWeaponTrigger);

      IsUnarmedMode = hideWeapon;

      _animator.SetInteger(animationsParameters.weaponInt, hideWeapon ? -1 : 0);
    }

    // Animation Event
    private void StartChangingWeapon()
    {
      IsSwapingWeapon = true;
    }

    // Animation Event
    private void FinishChangingWeapon()
    {
      IsSwapingWeapon = false;
    }

    // Animation Event
    private void UnequipEvent()
    {
      if (CurrentWeaponBehaviour)
      {
        CurrentWeaponBehaviour.gameObject.SetActive(false);
      }

      if (IsUnarmedMode)
      {
        CurrentWeaponIndex = -1;
        CurrentWeaponBehaviour = null;
        Events.PlayerHideWeapon.Call();
      }
      else
      {
        CurrentWeaponBehaviour = weaponSettings.AllWeapons[CurrentWeaponIndex];
        CurrentWeaponBehaviour.gameObject.SetActive(true);
        Events.PlayerShowWeapon.Call();
      }
    }

    public class SelectMenuWeaponUtil
    {
      private PlayerBehaviour player;

      public SelectMenuWeaponUtil(PlayerBehaviour player)
      {
        this.player = player;
      }

      public void SelectMenuWeapon()
      {
        string menuWeaponTag = SaveLoad.WeaponTag;
        if (menuWeaponTag.Length == 0) return;

        for (int i = 0; i < player.weaponSettings.AllWeapons.Length; ++i)
        {
          var weapon = player.weaponSettings.AllWeapons[i];
          if (weapon != null && weapon.gameObject.CompareTag(menuWeaponTag))
          {
            if (player.CurrentWeaponBehaviour != weapon)
              player.SetCurrentWeapon(i);
            break;
          }
        }
      }
    }
  }
}
