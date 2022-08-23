using UnityEngine;

using DG.Tweening;

namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    private class AttackState : EnemyBehaviourState
    {
      private bool canLookAtPlayer;
      private Vector3 lastShootPoint;

      private readonly Vector3 playerStandOffset = new Vector3(0, 1.5f, 0);
      private readonly Vector3 playerCrouchOffset = new Vector3(0, 1f, 0);

      public AttackState(EnemyBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.SetForwardAnimatorParameter(0);
        host.SetStrafeAnimatorParameter(0);
        host.StopNavMeshAgent();
        host.updateSpineIK = true;
        canLookAtPlayer = true;
      }

      public override void OnExit()
      {
        host.updateSpineIK = false;
      }

      public override void OnUpdate()
      {
        if (host.CanChangeStateToChase())
        {
          host.ChangeState(host.chaseState);
        }
        else if (IsPlayerLost())
        {
          if (host.AI_Behaviour.SearchSettings == SearchSettings.Search)
          {
            host.ChangeState(host.searchState);
          }
          else if (host.HasWaypoints())
          {
            host.ChangeState(host.patrolState);
          }
          else
          {
            host.ChangeState(host.idleState);
          }
        }
        else
        {
          Vector3 shootPoint = GetShootPoint();
          host.LookAtLerp(shootPoint);
          host.spineIKLookAt = shootPoint;
          if (host.WeaponSettings.Weapon.Fire(shootPoint))
          {
            canLookAtPlayer = true;
            host.DelayAction(
              host.WeaponSettings.Weapon.ShootFrequency - host.WeaponSettings.TimeBeforeShot,
              () => canLookAtPlayer = false,
              false
            );
          }
        }
      }

      private bool IsPlayerLost()
      {
        return host.IsPlayerNoticedByRaycast() == false;
      }

      private Vector3 GetShootPoint()
      {
        if (!canLookAtPlayer) return lastShootPoint;

        lastShootPoint = host.player.GetPosition() + (host.player.IsCrouching ? playerCrouchOffset : playerStandOffset);
        return lastShootPoint;
      }
    }
  }
}
