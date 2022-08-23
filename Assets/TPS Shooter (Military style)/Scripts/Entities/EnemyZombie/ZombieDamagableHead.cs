using LightDev;

namespace TPSShooter
{
  public class ZombieDamagableHead : EnemyDamagable
  {
    public override void OnBulletHit(PlayerBullet bullet)
    {
      base.OnBulletHit(bullet);

      Events.ZombieHeadshot.Call();
    }
  }
}
