namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    private class IdleState : EnemyBehaviourState
    {
      public IdleState(EnemyBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.StopNavMeshAgent();
        host.SetForwardAnimatorParameter(0);
        host.SetStrafeAnimatorParameter(0);
      }

      public override void OnUpdate()
      {
        if(host.CanChangeStateToSearch())
        {
          host.ChangeState(host.searchState);
        }
        else if(host.CanChangeStateToChase())
        {
          host.ChangeState(host.chaseState);
        }
        else if(host.CanChangeStateToAttack())
        {
          host.ChangeState(host.attackState);
        }
      }
    }
  }
}
