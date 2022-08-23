namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    private abstract class EnemyBehaviourState
    {
      protected EnemyBehaviour host;

      public EnemyBehaviourState(EnemyBehaviour host)
      {
        this.host = host;
      }

      public virtual void OnEnter()
      {
      }

      public virtual void OnExit()
      {
      }

      public virtual void OnUpdate()
      {
      }
    }
  }
}
