using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShooter
{
  [RequireComponent(typeof(Collider))]
  [RequireComponent(typeof(Rigidbody))]
  public abstract class PickUp : MonoBehaviour
  {
    private void OnValidate()
    {
      GetComponent<Rigidbody>().isKinematic = true;
      GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
      PlayerBehaviour player = other.GetComponentInParent<PlayerBehaviour>();
      if (player != null)
      {
        OnPlayerCollision(player);
      }
    }

    protected abstract void OnPlayerCollision(PlayerBehaviour player);
  }
}
