using UnityEngine;

using DG.Tweening;

namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    private class StrafeAttackState : AttackState
    {
      private float moveDirection = 1;

      private readonly float strafeSpeed = 3f;
      private readonly float strafeTime = 0.8f;
      private readonly string attackMotionSequence = "at";

      public StrafeAttackState(EnemyBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        base.OnEnter();

        host.updateSpineIK = false;
        MoveLeftRight();
      }

      public override void OnExit()
      {
        base.OnExit();

        host.KillSequence(attackMotionSequence);
      }

      public override void OnUpdate()
      {
        base.OnUpdate();

        host.characterController.Move(host.transform.right * moveDirection * strafeSpeed * Time.deltaTime);
      }

      private void MoveLeftRight()
      {
        if (host.AI_Behaviour.AttackMotion != AttackMotion.Strafe) return;

        host.Sequence(
          host.OnFinish(() =>
          {
            moveDirection *= -1;
            host.SetStrafeAnimatorParameter(moveDirection);
          }),
          host.Delay(strafeTime)
        ).SetLoops(-1).stringId = attackMotionSequence;
      }
    }
  }
}
