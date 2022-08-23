using UnityEngine;

using LightDev;

namespace TPSShooter
{
  public partial class ZombieBehaviour
  {
    private class DeathState : ZombieBehaviourState
    {
      public DeathState(ZombieBehaviour host) : base(host)
      {
      }

      public override void OnEnter()
      {
        host.characterController.enabled = false;
        host.navmeshAgent.enabled = false;

        host.animator.SetTrigger(ZombieBehaviour.DeathHash);

        // Event
        host.onDied?.Invoke();
        Events.ZobmieKilled.Call(host);

        // Destory game objects
        if (host.bloodEffect)
        {
          Destroy(host.bloodEffect.gameObject, host.bloodEffect.main.duration);
        }
        Destroy(host.gameObject, host.dieTime);
      }
    }
  }
}
