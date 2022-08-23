using UnityEngine;
using TPSShooter.UI;

using LightDev;

namespace TPSShooter
{
  [RequireComponent(typeof(EnemyBehaviour))]
  public class EnemyRadarableObject : RadarableObject
  {
    private EnemyBehaviour enemy;

    private void Awake()
    {
      enemy = GetComponent<EnemyBehaviour>();

      Events.EnemyKilled += OnEnemyKilled;
    }

    private void OnDestroy()
    {
      Events.EnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(EnemyBehaviour enemy)
    {
      if(this.enemy == enemy)
      {
        DestroyRadarableObject();
      }
    }
  }
}
