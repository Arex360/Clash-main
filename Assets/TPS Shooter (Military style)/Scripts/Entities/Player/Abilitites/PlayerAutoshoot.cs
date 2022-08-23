using UnityEngine;

using LightDev;

namespace TPSShooter
{
  [RequireComponent(typeof(PlayerBehaviour))]
  public class PlayerAutoshoot : MonoBehaviour
  {
    public bool isEnabled = true;
    public bool useSavedData = true;

    private PlayerBehaviour player;

    private void Awake()
    {
      player = GetComponent<PlayerBehaviour>();
      if (useSavedData)
      {
        isEnabled = SaveLoad.IsAutoShoot;
      }
    }

    private void Update()
    {
      if (IsAutoShootNeeded())
      {
        Events.FireRequested.Call();
      }
    }

    private bool IsAutoShootNeeded()
    {
      if (!isEnabled) return false;
      if (!player) return false;
      if (!player.FireHitObject) return false;

      var enemy = player.FireHitObject.GetComponentInParent<EnemyBehaviour>();
      if (enemy && enemy.IsAlive())
      {
        return true;
      }

      var zombie = player.FireHitObject.GetComponentInParent<ZombieBehaviour>();
      if (zombie && zombie.IsAlive())
      {
        return true;
      }

      return false;
    }
  }
}
