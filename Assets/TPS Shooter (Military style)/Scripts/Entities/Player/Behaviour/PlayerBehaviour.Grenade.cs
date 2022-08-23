using UnityEngine;

using LightDev;

namespace TPSShooter
{
  public partial class PlayerBehaviour
  {
    public bool IsThrowingGrenade { get; private set; }
    public Vector3 GetGreandeVelocity()
    {
      return Camera.main.transform.forward * 10 + new Vector3(0, 7, 0);
    }

    private GameObject _currentGrenade;

    private void OnGrenadeStartThrowRequest()
    {
      if (!IsAlive) return;
      if (grenadeSettings.GrenadePrefab == null) return;
      if (IsThrowingGrenade) return;
      if (IsReloading) return;
      if (IsDrivingVehicle) return;

      GrenadeStartThrow();
    }

    private void OnGrenadeFinishThrowRequest()
    {
      if (IsThrowingGrenade == false) return;

      GrenadeFinishThrow();
    }


    private void GrenadeStartThrow()
    {
      if (IsAiming)
        DeactivateAiming();

      // Player grenade animation
      _animator.SetTrigger(animationsParameters.standGrenadeTrigger);

      // sets player's state
      IsThrowingGrenade = true;

      // Create a grenade
      _currentGrenade = Instantiate(
        grenadeSettings.GrenadePrefab,
        grenadeSettings.GrenadePosition.position,
        grenadeSettings.GrenadePosition.rotation,
        grenadeSettings.GrenadeArmParent
      );
      _currentGrenade.GetComponent<AbstractGrenade>().OnReady();

      Events.PlayerStartGrenadeThrow.Call();
    }

    private void GrenadeFinishThrow()
    {
      _animator.SetTrigger(animationsParameters.standThrowGrenadeTrigger);
    }

    // Animation Event
    // Called when the player does not touch a grenade and it is starts flying
    private void GrenadeStartFlying()
    {
      if (_currentGrenade != null)
      {
        _currentGrenade.GetComponent<AbstractGrenade>().Throw(GetGreandeVelocity());
      }
      Events.PlayerFinishGreandeThrow.Call();
    }

    // Animation Event
    // Called when the Grenade animation finished
    private void GrenadeThrown()
    {
      IsThrowingGrenade = false;
    }
  }
}
