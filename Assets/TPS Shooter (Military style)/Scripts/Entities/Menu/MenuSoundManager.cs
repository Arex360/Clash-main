using UnityEngine;

using LightDev;

namespace TPSShooter
{
  public class MenuSoundManager : MonoBehaviour
  {
    public AudioSource clickSource;

    private void Awake()
    {
      Events.MenuClickSound += OnMenuClickSound;
    }

    private void OnDestroy()
    {
      Events.MenuClickSound -= OnMenuClickSound;
    }

    private void OnMenuClickSound()
    {
      clickSource.Play();
    }
  }
}
