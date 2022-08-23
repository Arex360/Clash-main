using UnityEngine;

using DG.Tweening;

namespace TPSShooter
{
  public partial class ZombieBehaviour
  {
    private class AttackState : ZombieBehaviourState
    {
      public AttackState(ZombieBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.animator.SetBool(ZombieBehaviour.AttackHash, true);
        host.StopNavMeshAgent();
      }

      public override void OnExit()
      {
        host.animator.SetBool(ZombieBehaviour.AttackHash, false);
      }

      public override void OnUpdate()
      {
        if (host.CanChangeStateToChase())
        {
          host.ChangeState(host.chaseState);
        }
        else if (IsPlayerLost())
        {
          host.ChangeState(host.searchState);
        }
        else
        {
          // TODO attack
        }
      }

      private bool IsPlayerLost()
      {
        return host.IsPlayerNoticedByRaycast() == false;
      }
    }
  }
}
