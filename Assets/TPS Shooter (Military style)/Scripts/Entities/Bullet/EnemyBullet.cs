using UnityEngine;

namespace TPSShooter
{
  public class EnemyBullet : AbstractBullet
  {
    // Can be used by hit marker to find position from where was shot
    public Transform MasterOfBullet { get; set; }

    protected override void OnBulletCollision(RaycastHit hit)
    {
      if (hit.transform.GetComponent<PlayerBehaviour>())
      {
        hit.transform.GetComponent<PlayerBehaviour>().OnBulletHit(this);
      }
    }
  }
}
