namespace TPSShooter
{
  public partial class ZombieBehaviour
  {
    private class PlayerDiedState : ZombieBehaviourState
    {
      public PlayerDiedState(ZombieBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.StopNavMeshAgent();
        host.animator.SetBool(ZombieBehaviour.WalkHash, false);
        host.animator.SetBool(ZombieBehaviour.RunHash, false);
        host.animator.SetBool(ZombieBehaviour.AttackHash, false);
      }
    }
  }
}
