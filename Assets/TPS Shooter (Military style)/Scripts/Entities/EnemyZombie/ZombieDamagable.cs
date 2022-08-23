using UnityEngine;

namespace TPSShooter
{
  /// <summary>
  /// Zombie part that can be damaged by Player Bullet.
  ///
  /// GameObject has to have EnemyDamagable layer.
  /// </summary>
  [RequireComponent(typeof(Collider))]
  public class ZombieDamagable : MonoBehaviour
  {
    [SerializeField] private float _damageMultiplier = 1f;

    public virtual void OnBulletHit(PlayerBullet bullet)
    {
      transform.GetComponentInParent<ZombieBehaviour>().OnBulletHit(bullet, _damageMultiplier);
    }
  }
}
