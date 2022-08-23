using UnityEngine;
using TPSShooter.UI;

using LightDev;

namespace TPSShooter
{
  [RequireComponent(typeof(ZombieBehaviour))]
  public class ZombieRadarableObject : RadarableObject
  {
    private ZombieBehaviour zombie;

    private void Awake()
    {
      zombie = GetComponent<ZombieBehaviour>();

      Events.ZobmieKilled += OnEnemyKilled;
    }

    private void OnDestroy()
    {
      Events.ZobmieKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(ZombieBehaviour zombie)
    {
      if(this.zombie == zombie)
      {
        DestroyRadarableObject();
      }
    }
  }
}
