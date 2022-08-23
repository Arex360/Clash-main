using UnityEngine;

using LightDev;

namespace TPSShooter
{
  public partial class EnemyBehaviour
  {
    private class DeathState : EnemyBehaviourState
    {
      public DeathState(EnemyBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.characterController.enabled = false;
        host.navmeshAgent.enabled = false;

        if (host.DeathSettings.IsRagdolled)
        {
          host.animator.enabled = false;

          Rigidbody[] bodies = host.GetComponentsInChildren<Rigidbody>();
          foreach (Rigidbody b in bodies)
            b.isKinematic = false;
        }
        else
        {
          host.SetForwardAnimatorParameter(0);
          host.SetStrafeAnimatorParameter(0);
          host.animator.SetTrigger(host.AnimatorParameters.DieHash);
        }

        // unattach gameObjects
        for (int i = 0; i < host.DeathSettings.Items.Length; i++)
        {
          host.DeathSettings.Items[i].parent = null;
          host.DeathSettings.Items[i].GetComponent<Rigidbody>().isKinematic = false;
          Destroy(host.DeathSettings.Items[i].gameObject, host.DeathSettings.EnemyDieTime);
        }

        // Event
        host.onDied?.Invoke();
        Events.EnemyKilled.Call(host);

        // Destory game objects
        if (host.BloodEffect)
        {
          Destroy(host.BloodEffect.gameObject, host.BloodEffect.main.duration);
        }
        Destroy(host.gameObject, host.DeathSettings.EnemyDieTime);
      }
    }
  }
}
