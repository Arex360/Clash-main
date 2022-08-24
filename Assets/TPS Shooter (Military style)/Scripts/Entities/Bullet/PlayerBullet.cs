using UnityEngine;

namespace TPSShooter
{
  public class PlayerBullet : AbstractBullet
  {
    public string id;
    public int bullet_damage = 1;
    protected override void OnBulletCollision(RaycastHit hit)
    {
      // Influence on hit Object
      if (hit.transform.GetComponent<Rigidbody>())
      {
        hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * 1800, hit.point);
      }

      if (hit.transform.GetComponent<EnemyDamagable>()) // Enemy damage
      {
        hit.transform.GetComponent<EnemyDamagable>().OnBulletHit(this);
      }
      else if (hit.transform.GetComponent<ZombieDamagable>()) // Zombie damage
      {
        hit.transform.GetComponent<ZombieDamagable>().OnBulletHit(this);
      }
      print($"Bullet shot by {id}");
      hit.transform.GetComponent<PlayerHealth>().TakeDamage(bullet_damage);
    }
  }
}
