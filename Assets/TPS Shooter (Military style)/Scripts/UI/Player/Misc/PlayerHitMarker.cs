using UnityEngine;

using LightDev;
using LightDev.UI;
using LightDev.Pool;

namespace TPSShooter.UI
{
  [RequireComponent(typeof(RectTransform))]
  public class PlayerHitMarker : CanvasElement
  {
    public GameObject hitMarkerPrefab;
    private PlayerBehaviour player;

    private void Start()
    {
      player = PlayerBehaviour.GetInstance();
    }

    public override void Subscribe()
    {
      Events.SceneLoaded += Show;
      Events.GamePaused += Hide;
      Events.GameResumed += Show;
      Events.PlayerDied += Hide;
      Events.PlayerBulletHit += OnPlayerBulletHit;
      Events.PlayerZombieHit += OnPlayerZombieHit;
    }

    public override void Unsubscribe()
    {
      Events.SceneLoaded -= Show;
      Events.GamePaused -= Hide;
      Events.GameResumed -= Show;
      Events.PlayerDied -= Hide;
      Events.PlayerBulletHit -= OnPlayerBulletHit;
      Events.PlayerZombieHit -= OnPlayerZombieHit;
    }

    private void OnPlayerBulletHit(EnemyBullet bullet)
    {
      CreateHitMarkerObject(bullet.MasterOfBullet.position);
    }

    private void OnPlayerZombieHit(ZombieBehaviour zombie)
    {
      CreateHitMarkerObject(zombie.GetPosition());
    }

    private void CreateHitMarkerObject(Vector3 enemyPos)
    {
      Vector3 relative = player.transform.InverseTransformPoint(enemyPos);
      float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

      GameObject marker = Instantiate(hitMarkerPrefab, transform);
      marker.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
      Destroy(marker, 1f);
    }
  }
}
