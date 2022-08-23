using UnityEditor;
using UnityEngine;

namespace TPSShooter
{
  [CustomEditor(typeof(PlayerVehicleAbility))]
  [CanEditMultipleObjects]
  public class PlayerVehicleAbilityEditor : Editor
  {
    private SerializedProperty tpsCameraProperty;
    private SerializedProperty hideSkinWhileDrivingProperty;
    private SerializedProperty skinProperty;

    private void OnEnable()
    {
      tpsCameraProperty = serializedObject.FindProperty("tpsCamera");
      hideSkinWhileDrivingProperty = serializedObject.FindProperty("hideSkinWhileDriving");
      skinProperty = serializedObject.FindProperty("skin");
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.PropertyField(tpsCameraProperty, new GUIContent("TPS Camera"));
      EditorGUILayout.PropertyField(hideSkinWhileDrivingProperty, new GUIContent("Hide Skin"));
      if (hideSkinWhileDrivingProperty.boolValue)
      {
        EditorGUILayout.PropertyField(skinProperty, new GUIContent("Skin"));
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
}
