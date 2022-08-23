using LightDev;

namespace TPSShooter
{
  public class EnemyDamagableHead : EnemyDamagable
  {    
    public override void OnBulletHit(PlayerBullet bullet)
    {
      base.OnBulletHit(bullet);
      
      Events.EnemyHeadshot.Call();
    }
  }
}
