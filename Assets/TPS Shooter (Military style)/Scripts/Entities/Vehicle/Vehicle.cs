using System.Collections;
using UnityEngine;

using LightDev;

namespace TPSShooter
{
  [RequireComponent(typeof(BoxCollider))]
  [RequireComponent(typeof(Rigidbody))]
  [RequireComponent(typeof(WheelDrive))]
  public class Vehicle : MonoBehaviour
  {
    public Transform playerSit;
    public Transform playerStand;
    public AudioSource idleSound;

    private Rigidbody _rigidbody;
    private WheelDrive _wheelDrive;

    public bool IsPlayerIn { get; private set; }

    private void Awake()
    {
      _wheelDrive = GetComponent<WheelDrive>();
      _rigidbody = GetComponent<Rigidbody>();

      Events.GamePaused += OnGamePaused;
      Events.GameResumed += OnGameResumed;
      Events.GameFinished += OnGameFinished;
    }

    private void OnDestroy()
    {
      Events.GamePaused -= OnGamePaused;
      Events.GameResumed -= OnGameResumed;
      Events.GameFinished -= OnGameFinished;
    }

    private void OnCollisionEnter(Collision collision)
    {
      if (collision.transform.GetComponentInParent<EnemyBehaviour>())
      {
        collision.transform.GetComponentInParent<EnemyBehaviour>().OnVehicleCollision();
      }
      else if (collision.transform.GetComponentInParent<ZombieBehaviour>())
      {
        collision.transform.GetComponentInParent<ZombieBehaviour>().OnVehicleCollision();
      }
    }

    private void OnGamePaused()
    {
      if (idleSound)
        idleSound.Stop();
    }

    private void OnGameResumed()
    {
      if (IsPlayerIn && idleSound)
      {
        idleSound.Play();
      }
    }

    private void OnGameFinished()
    {
      if (idleSound)
        idleSound.Stop();
    }

    public void PlayerGetIn()
    {
      IsPlayerIn = true;

      if (idleSound)
        idleSound.Play();
      _wheelDrive.DeactivateBrake();

      StartCoroutine(ChangeSoundPitch());
    }

    public void PlayerGetOut()
    {
      IsPlayerIn = false;

      if (idleSound)
        idleSound.Stop();
      _wheelDrive.Brake();
    }

    private IEnumerator ChangeSoundPitch()
    {
      while (IsPlayerIn && idleSound)
      {
        idleSound.pitch = Mathf.Min(2, 1 + _rigidbody.velocity.magnitude * 0.05f);

        yield return null;
      }
    }
  }
}
