using System.Collections;
using UnityEngine;

namespace TPSShooter
{
  public partial class ZombieBehaviour
  {
    private class PatrolState : ZombieBehaviourState
    {
      private int destinationIndex = 0;

      private const float MinStopDistance = 1;
      private const string WaitSequenceID = "patrol";

      public PatrolState(ZombieBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.navmeshAgent.speed = MaxWalkSpeed;

        UpdateDestination(destinationIndex);
        Move();
      }

      public override void OnExit()
      {
        host.animator.SetBool(ZombieBehaviour.WalkHash, false);
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
          float waitTime = host.waypoints[destinationIndex].WaitTime;
          destinationIndex = (destinationIndex + 1) % host.waypoints.Length;

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
        host.animator.SetBool(ZombieBehaviour.WalkHash, false);
      }

      private void Move()
      {
        host.ResumeNavMeshAgent();
        host.animator.SetBool(ZombieBehaviour.WalkHash, true);
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
        host.navmeshAgent.SetDestination(host.waypoints[index].Destination.position);
      }
    }
  }
}
