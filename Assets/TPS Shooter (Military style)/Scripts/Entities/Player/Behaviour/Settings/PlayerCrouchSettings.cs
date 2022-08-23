using UnityEngine;

namespace TPSShooter
{
  [System.Serializable]
  public class PlayerCrouchSettings
  {
    public float CharacterHeightCrouching = 1.2f;
    public Vector3 CharacterCenterCrouching = new Vector3(0, 0.6f, 0);
  }
}