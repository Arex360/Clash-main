using UnityEngine;

namespace TPSShooter
{
  public partial class TPSCamera
  {
    private class CameraPlayerState : CameraState
    {
      private Vector3 pivotCurrentLocalRotation;
      public Vector3 cachedContainerLocalPos;

      public CameraPlayerState(TPSCamera tpsCamera, Vector3 containerLocalPos) : base(tpsCamera)
      {
        cachedContainerLocalPos = containerLocalPos;
      }

      public override void OnEnter()
      {
        tpsCamera.transform.eulerAngles = new Vector3(0, tpsCamera.transform.eulerAngles.y, 0);
        pivotCurrentLocalRotation = tpsCamera.pivot.localEulerAngles;
        pivotCurrentLocalRotation.x = pivotCurrentLocalRotation.x.WrapAngle();
      }

      public override void OnUpdate()
      {
        UpdateCameraRigPosition();
        RotateCameraRig(InputController.HorizontalRotation);
        RotatePlayerPivot(InputController.VerticalRotation);
        UpdatePlayerRotation();
        CheckWallLayers();
        UpdateFOV();
      }

      private void UpdateCameraRigPosition()
      {
        tpsCamera.SetPosition(tpsCamera.target.position);
      }

      public void RotateCameraRig(float deltaY)
      {
        tpsCamera.transform.Rotate(0, deltaY, 0);
      }

      public void RotatePlayerPivot(float deltaX)
      {
        var player = PlayerBehaviour.GetInstance();
        Vector2 clamp = new Vector2(
          player.IsCrouching ? tpsCamera.playerCameraSettings.crouchMinAngle : tpsCamera.playerCameraSettings.standMinAngle,
          player.IsCrouching ? tpsCamera.playerCameraSettings.crouchMaxAngle : tpsCamera.playerCameraSettings.standMaxAngle
        );
        pivotCurrentLocalRotation.x -= deltaX;
        pivotCurrentLocalRotation.x = Mathf.Clamp(pivotCurrentLocalRotation.x, clamp.x, clamp.y);
        pivotCurrentLocalRotation.z = 0;

        tpsCamera.pivot.localRotation = Quaternion.Slerp(tpsCamera.pivot.localRotation, Quaternion.Euler(pivotCurrentLocalRotation), Time.deltaTime * 14f);
      }

      public void UpdatePlayerRotation()
      {
        Vector3 rotation = tpsCamera.target.transform.eulerAngles;
        rotation.y = tpsCamera.pivot.eulerAngles.y;
        tpsCamera.target.transform.eulerAngles = rotation;
      }

      private void CheckWallLayers()
      {
        RaycastHit hit;
        Vector3 pivotPos = tpsCamera.pivot.position;
        Vector3 dir = tpsCamera.cameraContainer.position - pivotPos;
        float dist = Mathf.Abs(cachedContainerLocalPos.z);

        if (Physics.SphereCast(pivotPos, 0.1f, dir, out hit, dist, tpsCamera.playerCameraSettings.wallLayers))
        {
          tpsCamera.cameraContainer.position = pivotPos + (dir.normalized * hit.distance); ;
        }
        else
        {
          tpsCamera.cameraContainer.localPosition = Vector3.Lerp(tpsCamera.cameraContainer.localPosition, cachedContainerLocalPos, Time.deltaTime * 100);
        }
      }

      private void UpdateFOV()
      {
        tpsCamera._camera.fieldOfView = Mathf.Lerp(tpsCamera._camera.fieldOfView, tpsCamera.playerCameraSettings.normalFieldOfView, 12 * Time.deltaTime);
      }
    }
  }
}
