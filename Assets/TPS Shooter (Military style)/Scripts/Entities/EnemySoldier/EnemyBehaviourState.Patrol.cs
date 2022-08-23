using System.Collections;
using UnityEngine;

namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    private class PatrolState : EnemyBehaviourState
    {
      private int destinationIndex = 0;

      private const float MinStopDistance = 1;
      private const string WaitSequenceID = "patrol";

      public PatrolState(EnemyBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.navmeshAgent.speed = MaxRunSpeed;
        host.navmeshAgent.acceleration = MaxRunAcceleration;

        UpdateDestination(destinationIndex);
        Move();
      }

      public override void OnExit()
      {
        host.KillSequence(WaitSequenceID);
      }

      public override void OnUpdate()
      {
        host.LookAtLerp(host.navmeshAgent.steeringTarget);

        if(host.CanChangeStateToAttack())
        {
          host.ChangeState(host.attackState);
        }
        else if(host.CanChangeStateToChase())
        {
          host.ChangeState(host.chaseState);
        }
        else if(host.CanChangeStateToSearch())
        {
          host.ChangeState(host.searchState);
        }
        else if (IsNearDestination())
        {
          float waitTime = host.PatrollingSettings.Waypoints[destinationIndex].WaitTime;
          destinationIndex = (destinationIndex + 1) % host.PatrollingSettings.Waypoints.Length;

          Stop();
          UpdateDestination(destinationIndex);
          MoveAfterTime(waitTime);
        }
      }

      private bool IsNearDestination()
      {
        return Vector3.Distance(host.navmeshAgent.destination, host.GetPosition()) < MinStopDistance;
      }

      private void Stop()
      {
        host.StopNavMeshAgent();
        host.SetForwardAnimatorParameter(0);
      }

      private void Move()
      {
        host.ResumeNavMeshAgent();
        host.SetForwardAnimatorParameter(1);
        host.SetStrafeAnimatorParameter(0);
      }

      private void MoveAfterTime(float time)
      {
        host.Sequence(
          host.Delay(time),
          host.OnFinish(() => Move())
        ).stringId = WaitSequenceID;
      }

      private void UpdateDestination(int index)
      {
        host.navmeshAgent.SetDestination(host.PatrollingSettings.Waypoints[index].Destination.position);
      }
    }
  }
}
