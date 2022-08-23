using UnityEngine;

namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    /// <summary>
    /// We can dive into this state only if Enemy SearchSettings set to Search.
    /// </summary>
    private class ChaseState : EnemyBehaviourState
    {
      public ChaseState(EnemyBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.navmeshAgent.speed = MaxRunSpeed;
        host.navmeshAgent.acceleration = MaxRunAcceleration;
        host.SetForwardAnimatorParameter(1);
        host.SetStrafeAnimatorParameter(0);
        host.ResumeNavMeshAgent();
        UpdateDestination();
      }

      public override void OnUpdate()
      {
        host.LookAtLerp(host.navmeshAgent.steeringTarget);

        if (host.CanChangeStateToAttack())
        {
          host.ChangeState(host.attackState);
        }
        else if (IsPlayerLost())
        {
          host.ChangeState(host.searchState);
        }
        else if (Time.frameCount % 4 == 0)
        {
          UpdateDestination();
        }
      }

      private bool IsPlayerLost()
      {
        return host.IsPlayerNoticedByRaycast() == false || host.MaxPlayerDetectionRadius < host.GetDistanceToPlayer();
      }

      private void UpdateDestination()
      {
        host.navmeshAgent.destination = PlayerBehaviour.GetInstance().GetPosition();
      }
    }
  }
}
