using UnityEngine;

namespace TPSShooter
{
  public partial class TPSCamera
  {
    [System.Serializable]
    public class VehicleCameraSettings
    {
      public float fieldOfView = 60;
      public float distanceToVehicle = 8f;
      public float normalVehiclePitch = 15f;
      public Vector2 vehiclePitchMinMax = new Vector2(-15, 45);

      // If player did not rotate camera for the some period of time (minTouchTime), camera will back to normal state 
      public float minTouchTime = 1.5f;

      public LayerMask wallLayers = 1 << 0;
    }
  }
}
