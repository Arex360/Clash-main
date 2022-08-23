using UnityEditor;
using UnityEngine;

namespace TPSShooter
{
  [CustomPropertyDrawer(typeof(PlayerWeapon.WeaponScopeSettings))]
  public class WeaponScopeSettingsDrawer : PropertyDrawer
  {
    private float height = EditorGUIUtility.singleLineHeight + 2;
    private float rectHeight = EditorGUIUtility.singleLineHeight;

    private const float additionalSpaceBetweenFPS = 10f;
    private const string IsAimingAvailableProperty = "IsAimingAvailable";
    private const string FieldOfViewProperty = "FieldOfView";
    private const string PlayerSpineRotationProperty = "PlayerSpineRotation";
    private const string CameraPositionProperty = "CameraPosition";
    private const string IsFPSProperty = "IsFPS";
    private const string WeaponRotationShootingProperty = "WeaponRotationShooting";
    private const string HandsProperty = "Hands";
    private const string WeaponParentProperty = "WeaponParent";
    private const string WeaponLocalPositionRotationProperty = "WeaponLocalPositionRotation";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // Expanding
      Rect foldoutRect = new Rect(position);
      foldoutRect.height = EditorGUIUtility.singleLineHeight;
      property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

      // Drawing
      EditorGUI.BeginProperty(position, label, property);
      if (property.isExpanded)
      {
        EditorGUI.indentLevel++;

        // Calculate Rects
        Rect isAimingAvailableRect = new Rect(position.x, position.y + height, position.width, rectHeight);

        Rect fieldOfViewRect = new Rect(position.x, isAimingAvailableRect.position.y + height, position.width, rectHeight);
        Rect cameraPositionRect = new Rect(position.x, fieldOfViewRect.position.y + height, position.width, rectHeight);
        Rect playerSpineRotationRect = new Rect(position.x, cameraPositionRect.position.y + height, position.width, rectHeight);

        Rect isFpsRect = new Rect(position.x, playerSpineRotationRect.position.y + height + additionalSpaceBetweenFPS, position.width, rectHeight);
        Rect weaponRotationShootingRect = new Rect(position.x, isFpsRect.y + height, position.width, rectHeight);
        Rect handsRect = new Rect(position.x, weaponRotationShootingRect.position.y + height, position.width, rectHeight);
        Rect weaponParentRect = new Rect(position.x, handsRect.position.y + height, position.width, rectHeight);
        Rect weaponLocalPosRotRect = new Rect(position.x, weaponParentRect.position.y + height, position.width, rectHeight);

        // Properties
        EditorGUI.PropertyField(isAimingAvailableRect, property.FindPropertyRelative(IsAimingAvailableProperty));
        if (property.FindPropertyRelative(IsAimingAvailableProperty).boolValue)
        {
          EditorGUI.indentLevel++;
          EditorGUI.PropertyField(fieldOfViewRect, property.FindPropertyRelative(FieldOfViewProperty));
          EditorGUI.PropertyField(playerSpineRotationRect, property.FindPropertyRelative(PlayerSpineRotationProperty));
          EditorGUI.PropertyField(cameraPositionRect, property.FindPropertyRelative(CameraPositionProperty));
          EditorGUI.indentLevel--;
          EditorGUI.PropertyField(isFpsRect, property.FindPropertyRelative(IsFPSProperty));
          EditorGUI.indentLevel++;
          if (property.FindPropertyRelative(IsFPSProperty).boolValue)
          {
            EditorGUI.PropertyField(weaponRotationShootingRect, property.FindPropertyRelative(WeaponRotationShootingProperty));
            EditorGUI.PropertyField(handsRect, property.FindPropertyRelative(HandsProperty));
            EditorGUI.PropertyField(weaponParentRect, property.FindPropertyRelative(WeaponParentProperty));
            EditorGUI.PropertyField(weaponLocalPosRotRect, property.FindPropertyRelative(WeaponLocalPositionRotationProperty));
          }
          EditorGUI.indentLevel--;
        }
      }
      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      float result = height;

      if(property.isExpanded)
      {
        result += height;
        if (property.FindPropertyRelative(IsAimingAvailableProperty).boolValue)
        {
          result += 4 * height + additionalSpaceBetweenFPS;
          if(property.FindPropertyRelative(IsFPSProperty).boolValue)
          {
            result += 4 * height;
          }
        }
      }

      return result;
    }
  }
}