using UnityEngine;

namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    private class SearchState : EnemyBehaviourState
    {
      private Vector3 playerPosition;
      private float expiredTimeToAttackState;

      private const float stopDistance = 0.5f;
      private const float timeToStop = 0.3f;

      public SearchState(EnemyBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.navmeshAgent.speed = MaxRunSpeed;
        host.navmeshAgent.acceleration = MaxRunAcceleration;
        host.ResumeNavMeshAgent();
        host.SetForwardAnimatorParameter(1);
        host.SetStrafeAnimatorParameter(0);
        RecalculateDestination();

        expiredTimeToAttackState = timeToStop;
      }

      public override void OnUpdate()
      {
        host.LookAtLerp(host.navmeshAgent.steeringTarget);

        if (CanChangeStateToIdle())
        {
          host.ChangeState(host.idleState);
        }
        else if (CanChangeStateToAttack())
        {
          host.ChangeState(host.attackState);
        }
        else if (host.CanChangeStateToChase())
        {
          host.ChangeState(host.chaseState);
        }
        else if (IsNeedRecalculation())
        {
          RecalculateDestination();
        }
      }

      private bool CanChangeStateToAttack()
      {
        bool canChangeToAttack = host.CanChangeStateToAttack();
        expiredTimeToAttackState = canChangeToAttack ? (expiredTimeToAttackState - Time.deltaTime) : timeToStop;

        return expiredTimeToAttackState < 0;
      }

      private bool CanChangeStateToIdle()
      {
        if (Vector3.Distance(playerPosition, host.GetPosition()) > stopDistance) return false;
        if (host.IsPlayerNoticedByRaycast() && host.GetDistanceToPlayer() < host.MaxPlayerDetectionRadius) return false;

        return true;
      }

      private bool IsNeedRecalculation()
      {
        return host.IsPlayerNoiseDetected() && !host.IsPlayerNoticedByRaycast();
      }

      private void RecalculateDestination()
      {
        playerPosition = host.player.GetPosition();
        host.navmeshAgent.SetDestination(playerPosition);
      }
    }
  }
}
