using UnityEngine;
using LightDev;

namespace TPSShooter
{
  public partial class PlayerBehaviour
  {
    private const float MaxHP = 100;
    private float hp = MaxHP;

    public float GetMaxHP() { return MaxHP; }
    public float GetCurrentHP() { return hp; }

    public void OnBulletHit(EnemyBullet bullet)
    {
      sounds.PlaySound(sounds.HitSound);
      DecreaseHP(bullet.damage);
      Events.PlayerBulletHit.Call(bullet);
    }

    public void OnGrenadeHit(AbstractGrenade grenade)
    {
      if (Vector3.Distance(transform.position, grenade.transform.position) < 2)
      {
        Die();
      }
    }

    public void OnZombieHit(ZombieBehaviour zombie)
    {
      sounds.PlaySound(sounds.HitSound);
      DecreaseHP(zombie.damage);
      Events.PlayerZombieHit.Call(zombie);
    }

    public void IncreaseHP(float deltaHP)
    {
      AddDeltaHP(deltaHP);
    }

    public void DecreaseHP(float hp)
    {
      AddDeltaHP(-hp);
    }

    private void AddDeltaHP(float deltaHP)
    {
      hp += deltaHP;
      hp = Mathf.Clamp(hp, 0, MaxHP);
      Events.PlayerChangedHP.Call();

      if (hp <= 0)
        Die();
    }
  }
}
