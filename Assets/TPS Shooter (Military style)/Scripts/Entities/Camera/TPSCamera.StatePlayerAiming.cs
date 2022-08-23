using UnityEngine;

using DG.Tweening;

namespace TPSShooter
{
  public partial class TPSCamera
  {
    private class CameraPlayerAimingState : CameraState
    {
      private Vector3 pivotCurrentLocalRotation;

      private const string StartMovementSequence = "aimS";

      public CameraPlayerAimingState(TPSCamera tpsCamera) : base(tpsCamera)
      {
      }

      public override void OnEnter()
      {
        pivotCurrentLocalRotation = tpsCamera.pivot.localEulerAngles;
        pivotCurrentLocalRotation.x = pivotCurrentLocalRotation.x.WrapAngle();

        tpsCamera.Sequence(
          tpsCamera.cameraContainer.DOLocalMove(
            PlayerBehaviour.GetInstance().CurrentWeaponBehaviour.ScopeSettings.CameraPosition.localPosition,
            0.1f
          ).SetEase(Ease.OutSine)
        ).stringId = StartMovementSequence;
        tpsCamera.Sequence(
          tpsCamera._camera.DOFieldOfView(
            PlayerBehaviour.GetInstance().CurrentWeaponBehaviour.ScopeSettings.FieldOfView,
            0.1f
          ).SetEase(Ease.OutSine)
        ).stringId = StartMovementSequence;
      }

      public override void OnExit()
      {
        tpsCamera.KillSequence(StartMovementSequence);
      }

      public override void OnUpdate()
      {
        UpdateCameraRigPosition();
        RotateCameraRig(InputController.HorizontalRotation);
        RotatePlayerPivot(InputController.VerticalRotation);
        UpdatePlayerRotation();
        CheckWallLayers();
      }

      private void UpdateCameraRigPosition()
      {
        tpsCamera.SetPosition(tpsCamera.target.position);
      }

      private void CheckWallLayers()
      {
        Vector3 localPos = PlayerBehaviour.GetInstance().CurrentWeaponBehaviour.ScopeSettings.CameraPosition.localPosition;
        RaycastHit hit;
        Vector3 pivotPos = tpsCamera.pivot.position;
        Vector3 dir = tpsCamera.cameraContainer.position - pivotPos;
        float dist = Mathf.Abs(localPos.z);

        if (Physics.SphereCast(pivotPos, 0.1f, dir, out hit, dist, tpsCamera.playerCameraSettings.wallLayers))
        {
          tpsCamera.cameraContainer.position = pivotPos + (dir.normalized * hit.distance); ;
        }
        else
        {
          tpsCamera.cameraContainer.localPosition = Vector3.Lerp(tpsCamera.cameraContainer.localPosition, localPos, Time.deltaTime * 100);
        }
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
    }
  }
}
