using UnityEngine;

namespace TPSShooter
{
  /// <summary>
  /// Enemy part that can be damaged by Player Bullet.
  ///
  /// GameObject has to have EnemyDamagable layer.
  /// </summary>
  [RequireComponent(typeof(Collider))]
  public class EnemyDamagable : MonoBehaviour
  {
    [SerializeField] private float _damageMultiplier = 1f;
    
    public virtual void OnBulletHit(PlayerBullet bullet)
    {
      transform.GetComponentInParent<EnemyBehaviour>().OnBulletHit(bullet, _damageMultiplier);
    }
  }
}
