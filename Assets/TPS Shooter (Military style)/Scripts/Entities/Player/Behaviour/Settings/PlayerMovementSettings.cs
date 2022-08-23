using UnityEngine;

namespace TPSShooter
{
  // All speed in meters/seconds
  [System.Serializable]
  public class PlayerMovementSettings
  {
    public LayerMask GroundLayers = 1 << 0;

    public float AirSpeed = 5f; // forward and strafe but in the air
    public float JumpSpeed = 8; // speed along Y that will be applied during JumpTime
    public float JumpTime = 0.25f;

    public bool ApplyRootMotion;

    public float ForwardSpeed = 4;
    public float StrafeSpeed = 4;
    public float SprintSpeed = 7;

    public float CrouchForwardSpeed = 2.5f;
    public float CrouchStrafeSpeed = 2.5f;
  }
}