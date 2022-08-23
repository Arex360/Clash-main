using UnityEngine;

namespace TPSShooter
{
  public partial class ZombieBehaviour
  {
    private class ChaseState : ZombieBehaviourState
    { 
      public ChaseState(ZombieBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.navmeshAgent.speed = MaxRunSpeed;
        host.animator.SetBool(ZombieBehaviour.RunHash, true);
        host.ResumeNavMeshAgent();
        UpdateDestination();
      }

      public override void OnExit()
      {
        host.animator.SetBool(ZombieBehaviour.RunHash, false);
      }

      public override void OnUpdate()
      {
        host.LookAtLerp(host.navmeshAgent.steeringTarget);

        if(host.CanChangeStateToAttack())
        {
          host.ChangeState(host.attackState);
        }
        else if(IsPlayerLost())
        {
          host.ChangeState(host.searchState);
        }
        else if(Time.frameCount % 4 == 0)
        {
          UpdateDestination();
        }
      }

      private bool IsPlayerLost()
      {
        return host.IsPlayerNoticedByRaycast() == false || host.playerDetectionRadius < host.GetDistanceToPlayer();
      }

      private void UpdateDestination()
      {
        host.navmeshAgent.destination = PlayerBehaviour.GetInstance().GetPosition();
      }
    }
  }
}
