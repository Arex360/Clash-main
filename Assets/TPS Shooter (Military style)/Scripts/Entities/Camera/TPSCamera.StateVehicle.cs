using UnityEngine;

namespace TPSShooter
{
  public partial class TPSCamera
  {
    private class CameraVehicleState : CameraState
    {
      private Transform vehicle;
      private Vector3 currentPivotRotation;

      private Vector3 cachedPivotLocalPos;

      private float nonTouchTime;

      public CameraVehicleState(TPSCamera tpsCamera) : base(tpsCamera)
      {
      }

      public override void OnEnter()
      {
        vehicle = PlayerBehaviour.GetInstance().DrivingVehicle.transform;

        cachedPivotLocalPos = tpsCamera.pivot.localPosition;
        nonTouchTime = tpsCamera.vehicleCameraSettings.minTouchTime;

        Vector3 cameraPos = tpsCamera.cameraContainer.position;
        Vector3 cameraRotation = tpsCamera.pivot.eulerAngles;

        tpsCamera.transform.position = vehicle.position;
        tpsCamera.transform.rotation = vehicle.rotation;

        tpsCamera.pivot.localPosition = Vector3.zero;
        tpsCamera.pivot.eulerAngles = cameraRotation;

        tpsCamera.cameraContainer.transform.position = cameraPos;

        currentPivotRotation = tpsCamera.pivot.localEulerAngles;
        currentPivotRotation.z = 0;
      }

      public override void OnExit()
      {
        tpsCamera.pivot.localPosition = cachedPivotLocalPos;
      }

      public override void OnUpdate()
      {
        UpdateTouchTime();
        RotateVehiclePivot();
        CheckWallVehicle();
        UpdateFOV();
      }

      private void UpdateTouchTime()
      {
        if (InputController.HorizontalRotation == 0 && InputController.VerticalRotation == 0)
        {
          nonTouchTime += Time.deltaTime;
        }
        else
        {
          nonTouchTime = 0;
        }
      }

      private void RotateVehiclePivot()
      {
        tpsCamera.transform.position = vehicle.position;
        tpsCamera.transform.rotation = vehicle.rotation;

        if (nonTouchTime >= tpsCamera.vehicleCameraSettings.minTouchTime)
        {
          tpsCamera.pivot.localRotation = Quaternion.Lerp(tpsCamera.pivot.localRotation, Quaternion.Euler(tpsCamera.vehicleCameraSettings.normalVehiclePitch, 0, 0), 8 * Time.deltaTime);
          currentPivotRotation = tpsCamera.pivot.localEulerAngles;
        }
        else
        {
          currentPivotRotation.y += InputController.HorizontalRotation;
          currentPivotRotation.x -= InputController.VerticalRotation;
          currentPivotRotation.x = Mathf.Clamp(currentPivotRotation.x, tpsCamera.vehicleCameraSettings.vehiclePitchMinMax.x, tpsCamera.vehicleCameraSettings.vehiclePitchMinMax.y);
          currentPivotRotation.z = 0;
          tpsCamera.pivot.localRotation = Quaternion.Lerp(tpsCamera.pivot.localRotation, Quaternion.Euler(currentPivotRotation), 800 * Time.deltaTime);
        }
      }

      private void CheckWallVehicle()
      {
        RaycastHit hit;
        Vector3 startPos = tpsCamera.pivot.position;
        Vector3 direction = -tpsCamera.pivot.forward * tpsCamera.vehicleCameraSettings.distanceToVehicle;

        if (Physics.SphereCast(startPos, 0.1f, direction, out hit, direction.magnitude, tpsCamera.vehicleCameraSettings.wallLayers))
        {
          tpsCamera.cameraContainer.position = startPos + (direction.normalized * hit.distance);
        }
        else
        {
          tpsCamera.cameraContainer.position = Vector3.Lerp(tpsCamera.cameraContainer.position, startPos + direction, 14 * Time.deltaTime);
        }
      }

      private void UpdateFOV()
      {
        tpsCamera._camera.fieldOfView = Mathf.Lerp(tpsCamera._camera.fieldOfView, tpsCamera.vehicleCameraSettings.fieldOfView, 2 * Time.deltaTime);
      }
    }
  }
}
