namespace TPSShooter
{
  public partial class ZombieBehaviour
  {
    private abstract class ZombieBehaviourState
    {
      protected ZombieBehaviour host;

      public ZombieBehaviourState(ZombieBehaviour host)
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
