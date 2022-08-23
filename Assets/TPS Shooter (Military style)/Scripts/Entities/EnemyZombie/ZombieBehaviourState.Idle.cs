namespace TPSShooter
{
  public partial class ZombieBehaviour
  {
    private class IdleState : ZombieBehaviourState
    {
      public IdleState(ZombieBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.StopNavMeshAgent();
        host.animator.SetBool(ZombieBehaviour.RunHash, false);
        host.animator.SetBool(ZombieBehaviour.WalkHash, false);
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
