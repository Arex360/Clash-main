using UnityEngine;

namespace TPSShooter
{
  [RequireComponent(typeof(WheelDrive))]
  public class VehicleHealthBar : MonoBehaviour
  {
    public AudioSource explosionSound;
    public ParticleSystem explosionParticle;

    public bool WasExplode { get; private set; }

    public void Explode()
    {
      if (WasExplode) return;

      WasExplode = true;
      GetComponent<WheelDrive>().Brake();
      if (explosionParticle) explosionParticle.Play();
      if (explosionSound) explosionSound.Play();
    }
  }
}
