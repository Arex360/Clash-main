using TPSShooter;

namespace LightDev
{
  public partial class Events
  {
    public static Event<EnemyBehaviour> EnemyCreated;
    public static Event<EnemyBehaviour> EnemyKilled;
    public static Event EnemyHeadshot;
  }
}
