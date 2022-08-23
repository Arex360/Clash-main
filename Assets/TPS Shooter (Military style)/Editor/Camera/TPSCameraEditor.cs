using UnityEngine;
using UnityEditor;

namespace TPSShooter
{
  [CustomEditor(typeof(TPSCamera))]
  [CanEditMultipleObjects]
  public class TPSCameraEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      CheckCameraComponent();
    }

    private void CheckCameraComponent()
    {
      var cameraTransform = ((TPSCamera)target).cameraTransform;
      if (cameraTransform != null && cameraTransform.GetComponent<Camera>() == null)
      {
        EditorGUILayout.HelpBox("CameraTransform has to have Camera component.", MessageType.Error);
      }
    }
  }
}
