using UnityEngine;

namespace TPSShooter
{
  public partial class TPSCamera
  {
    [System.Serializable]
    public class PlayerCameraSettings
    {
      [Header("- Camera Options -")]
      public LayerMask wallLayers = 1 << 0;
      public float normalFieldOfView = 60;

      [Header("- Stand settings -")]
      public float standMinAngle = -30;
      public float standMaxAngle = 70;

      [Header("- Crouch settings -")]
      public float crouchMinAngle = -30;
      public float crouchMaxAngle = 55;
      public Vector3 crouchPivotPositionOffset = new Vector3(0, -0.4f, 0);
    }
  }
}
