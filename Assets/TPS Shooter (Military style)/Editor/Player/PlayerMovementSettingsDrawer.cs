using UnityEditor;
using UnityEngine;

namespace TPSShooter
{
  [CustomPropertyDrawer(typeof(PlayerMovementSettings))]
  public class PlayerMovementSettingsDrawer : PropertyDrawer
  {
    private readonly float height = EditorGUIUtility.singleLineHeight + 2;

    private const string GroundLayersProperty = "GroundLayers";
    private const string AirSpeedProperty = "AirSpeed";
    private const string JumpSpeedProperty = "JumpSpeed";
    private const string JumpTimeProperty = "JumpTime";
    private const string ApplyRootMotionProperty = "ApplyRootMotion";
    private const string ForwardSpeedProperty = "ForwardSpeed";
    private const string StrafeSpeedProperty = "StrafeSpeed";
    private const string SprintSpeedProperty = "SprintSpeed";
    private const string CrouchForwardSpeedProperty = "CrouchForwardSpeed";
    private const string CrouchStrafeSpeedProperty = "CrouchStrafeSpeed";

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

        float rectHeight = EditorGUIUtility.singleLineHeight;

        // Calculate Rects
        Rect groundLayersRect = new Rect(position.x, position.y + height * 1, position.width, rectHeight);

        Rect airControlHeaderRect = new Rect(position.x, groundLayersRect.y + height * 1.5f, position.width, rectHeight);
        Rect airSpeedRect = new Rect(position.x, airControlHeaderRect.y + height * 1, position.width, rectHeight);
        Rect jumpSpeedRect = new Rect(position.x, airSpeedRect.y + height * 1, position.width, rectHeight);
        Rect jumpTimeRect = new Rect(position.x, jumpSpeedRect.y + height * 1, position.width, rectHeight);

        Rect applyRootMotionHeaderRect = new Rect(position.x, jumpTimeRect.y + height * 1.5f, position.width, rectHeight);
        Rect applyRootMotionRect = new Rect(position.x, applyRootMotionHeaderRect.y + height * 1, position.width, rectHeight);

        Rect forwardSpeedRect = new Rect(position.x, applyRootMotionRect.y + height * 1, position.width, rectHeight);
        Rect strageSpeedRect = new Rect(position.x, forwardSpeedRect.y + height * 1, position.width, rectHeight);
        Rect sprintSpeedRect = new Rect(position.x, strageSpeedRect.y + height * 1, position.width, rectHeight);

        Rect crouchForwardSpeedRect = new Rect(position.x, sprintSpeedRect.y + height * 1.5f, position.width, rectHeight);
        Rect crouchStrafeSpeedRect = new Rect(position.x, crouchForwardSpeedRect.y + height * 1f, position.width, rectHeight);

        // Ground Layers property
        EditorGUI.PropertyField(groundLayersRect, property.FindPropertyRelative(GroundLayersProperty));

        // Header and Air Control properties
        EditorGUI.LabelField(airControlHeaderRect, "- AIR CONTROL -", UnityEditor.EditorStyles.boldLabel);
        EditorGUI.PropertyField(airSpeedRect, property.FindPropertyRelative(AirSpeedProperty));
        EditorGUI.PropertyField(jumpSpeedRect, property.FindPropertyRelative(JumpSpeedProperty));
        EditorGUI.PropertyField(jumpTimeRect, property.FindPropertyRelative(JumpTimeProperty));

        // Header and ApplyRootMotion property
        EditorGUI.LabelField(applyRootMotionHeaderRect, "- ROOT MOTION or IN PLACE -", UnityEditor.EditorStyles.boldLabel);
        EditorGUI.PropertyField(applyRootMotionRect, property.FindPropertyRelative(ApplyRootMotionProperty));

        // In place animation properties
        if (property.FindPropertyRelative(ApplyRootMotionProperty).boolValue == false)
        {
          EditorGUI.indentLevel++;

          EditorGUI.PropertyField(forwardSpeedRect, property.FindPropertyRelative(ForwardSpeedProperty));
          EditorGUI.PropertyField(strageSpeedRect, property.FindPropertyRelative(StrafeSpeedProperty));
          EditorGUI.PropertyField(sprintSpeedRect, property.FindPropertyRelative(SprintSpeedProperty));

          EditorGUI.PropertyField(crouchForwardSpeedRect, property.FindPropertyRelative(CrouchForwardSpeedProperty));
          EditorGUI.PropertyField(crouchStrafeSpeedRect, property.FindPropertyRelative(CrouchStrafeSpeedProperty));

          EditorGUI.indentLevel--;
        }

        EditorGUI.indentLevel--;
      }
      EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      float result = height;

      if (property.isExpanded)
      {
        result += height * (1 + 1.5f + 3 + 1.5f + 1);
        if(property.FindPropertyRelative(ApplyRootMotionProperty).boolValue == false)
        {
          result += height * (3 + 1.5f + 1);
        }
      }

      return result;
    }
  }
}
