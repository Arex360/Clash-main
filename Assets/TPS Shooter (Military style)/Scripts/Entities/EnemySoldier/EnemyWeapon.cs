using UnityEngine;
using System.Collections;

using LightDev.Core;

namespace TPSShooter
{
  public class EnemyWeapon : Base
  {
    [Header("- Bullet settings -")]
    public GameObject BulletPrefab;
    public Transform BulletPosition;

    [Header("- Gun settings -")]
    public float ShootFrequency = 0.14f;

    [Header("- Sound -")]
    public AudioSource FireSound;

    [Header("- Fire particle -")]
    public ParticleSystem FireParticleSystem;

    public bool CanShoot { get; private set; } = true;

    public bool Fire(Vector3 positionWhereToFire)
    {
      if(!CanShoot) return false;

      // makes shooting unavailable for shootFrequency time
      CanShoot = false;
      DelayAction(ShootFrequency, () => CanShoot = true, false);

      // Sound
      FireSound?.Stop();
      FireSound?.Play();

      // Particle
      FireParticleSystem?.Stop();
      FireParticleSystem?.Play();

      // Bullet position
      BulletPosition.LookAt(positionWhereToFire);

      // Instantiates bullet
      GameObject bullet = Instantiate(
        BulletPrefab,
        BulletPosition.transform.position,
        BulletPosition.transform.rotation
      );
      bullet.GetComponent<EnemyBullet>().MasterOfBullet = transform;

      return true;
    }
  }
}
