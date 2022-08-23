namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    private class PlayerDiedState : EnemyBehaviourState
    {
      public PlayerDiedState(EnemyBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.StopNavMeshAgent();
        host.SetForwardAnimatorParameter(0);
        host.SetStrafeAnimatorParameter(0);
      }
    }
  }
}
