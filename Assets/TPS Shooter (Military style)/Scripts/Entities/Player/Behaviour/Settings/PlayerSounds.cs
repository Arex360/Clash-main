using UnityEngine;

namespace TPSShooter
{
  [System.Serializable]
  public class PlayerSounds
  {
    public AudioSource AudioSource;

    [Header("- JUMP -")]
    public AudioClip JumpSound;
    public AudioClip LandSound;

    [Header("- ZOOM -")]
    public AudioClip ZoomInSound;
    public AudioClip ZoomOutSound;

    [Header("- WEAPON CHANGE -")]
    public AudioClip ChangeWeaponSound;

    [Header("- HP -")]
    public AudioClip HitSound;

    public void PlaySound(AudioClip clip)
    {
      if (clip == null)
      {
        Debug.LogError("PlayerBehaviour: AudioClip is not settuped.");
        return;
      }

      AudioSource.clip = clip;
      AudioSource.Play();
    }
  }
}
