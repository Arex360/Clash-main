using System.Collections;
using UnityEngine;

namespace TPSShooter
{
  [RequireComponent(typeof(Rigidbody))]
  [RequireComponent(typeof(SphereCollider))]
  public abstract class AbstractGrenade : MonoBehaviour
  {
    [Header("Settings")]
    public float delay = 5f;
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;

    [Header("Effects")]
    public ParticleSystem explosionParticle;
    public AudioSource explosionSound;

    protected float currentDelay;

    private bool isInHand;
    private Vector3 localPos;
    private Vector3 localRot;

    private void FixedUpdate()
    {
      UpdatePosition();
    }

    public void OnReady()
    {
      isInHand = true;
      localPos = transform.localPosition;
      localRot = transform.localEulerAngles;
      UpdatePosition();
      GetComponent<Rigidbody>().isKinematic = true;
      StartCoroutine(ExplosionTimer());
    }

    public void Throw(Vector3 velocity)
    {
      isInHand = false;
      transform.parent = null;
      GetComponent<Rigidbody>().isKinematic = false;
      GetComponent<Rigidbody>().velocity = velocity;
    }

    private void UpdatePosition()
    {
      if (isInHand)
      {
        transform.localPosition = localPos;
        transform.localEulerAngles = localRot;
      }
    }

    private IEnumerator ExplosionTimer()
    {
      currentDelay = delay;
      while (currentDelay > 0)
      {
        currentDelay -= Time.deltaTime;
        yield return null;
      }

      Explode();
    }

    private void Explode()
    {
      // explosion effect
      explosionParticle.transform.parent = null;
      explosionParticle.Play();
      Destroy(explosionParticle.gameObject, explosionParticle.main.duration);

      // Play sound
      explosionSound.transform.parent = null;
      explosionSound.Play();
      Destroy(explosionSound.gameObject, explosionSound.clip.length);

      Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, explosionRadius);

      // impact on nearby objects
      CollidersImpact(nearbyColliders);

      AbstractImpact(nearbyColliders);

      // Destroy the grenade
      Destroy(gameObject);
    }

    private void CollidersImpact(Collider[] nearbyColliders)
    {
      foreach (Collider nearbyObject in nearbyColliders)
      {
        Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

        if (rb != null)
          rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
      }
    }

    protected abstract void AbstractImpact(Collider[] nearbyColliders);
  }
}
