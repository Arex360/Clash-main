using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TPSShooter
{
  [RequireComponent(typeof(ZombieBehaviour))]
  public class ZombieBehaviourDebug : MonoBehaviour
  {
#if UNITY_EDITOR

    public bool visualizeOnlySelected;

    private PlayerBehaviour player;
    private ZombieBehaviour zombie;
    private ZombieBehaviour.ZombieBehaviourDebugFriend friend;

    private static int previousFrame;

    private void Start()
    {
      friend = new ZombieBehaviour.ZombieBehaviourDebugFriend(GetComponent<ZombieBehaviour>());
      player = PlayerBehaviour.GetInstance();
    }

    private void OnDrawGizmosSelected()
    {
      if (visualizeOnlySelected == true)
      {
        Draw();
        DrawPlayerNoise();
      }
    }

    private void OnDrawGizmos()
    {
      if (visualizeOnlySelected == false)
      {
        Draw();
        DrawPlayerNoise();
      }
    }

    private void Draw()
    {
      if (zombie == null)
      {
        zombie = GetComponent<ZombieBehaviour>();
      }

      // Save settings
      var zTestBefore = Handles.zTest;
      var handlesColorBefore = Handles.color;
      var gizmosColorBefore = Gizmos.color;

      Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

      // Draw player detection radius
      Handles.color = Color.blue.SetAlpha(0.5f);
      Handles.DrawSolidDisc(transform.position, Vector3.up, zombie.playerDetectionRadius);

      if (friend != null)
      {
        // Draw raycast
        Gizmos.color = Color.black;
        Gizmos.DrawLine(friend.GetVisionPos(), friend.GetPlayerVisionPos());
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(friend.GetHitPos().point, 0.3f);

        // Draw current attack radius
        Handles.color = Color.yellow.SetAlpha(0.5f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, ZombieBehaviour.OuterAttackRadius);

        Handles.color = Color.red.SetAlpha(0.5f);
        float radius = friend.IsAttacking() ? ZombieBehaviour.OuterAttackRadius : ZombieBehaviour.InnerAttackRadius;
        Handles.DrawSolidDisc(transform.position, Vector3.up, radius);
      }
      else // Draw attack radius borders, when application is not running
      {
        Handles.color = Color.yellow.SetAlpha(0.5f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, ZombieBehaviour.OuterAttackRadius);

        Handles.color = Color.red.SetAlpha(0.5f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, ZombieBehaviour.InnerAttackRadius);
      }

      // Draw vision zone
      Vector3 dir = transform.forward;
      dir.y = 0;
      dir = Quaternion.Euler(0, -zombie.fov, 0) * dir;
      Handles.color = Color.green.SetAlpha(0.5f);
      Handles.DrawSolidArc(transform.position, Vector3.up, dir, zombie.fov * 2, zombie.playerDetectionRadius);

      // Restore Handles settings
      Handles.zTest = zTestBefore;
      Handles.color = handlesColorBefore;
      Gizmos.color = gizmosColorBefore;
    }

    private void DrawPlayerNoise()
    {
      if (Time.frameCount == previousFrame || player == null)
      {
        return;
      }

      previousFrame = Time.frameCount;

      // Save settings
      var zTestBefore = Handles.zTest;
      var handlesColorBefore = Handles.color;

      // Draw player noise radius
      Handles.color = Color.black.SetAlpha(0.3f);
      Handles.DrawSolidDisc(player.GetPosition(), Vector3.up, player.Noise);

      // Restore Handles settings
      Handles.zTest = zTestBefore;
      Handles.color = handlesColorBefore;
    }

#endif
  }
}
