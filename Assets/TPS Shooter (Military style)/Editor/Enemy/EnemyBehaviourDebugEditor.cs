using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TPSShooter
{
  [CustomEditor(typeof(EnemyBehaviourDebug))]
  [CanEditMultipleObjects]
  public class EnemyBehaviourDebugEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      GUIStyle myStyle = GUI.skin.GetStyle("HelpBox");
      myStyle.richText = true;

      EditorGUILayout.TextArea(
        "<b>Black</b>   -> player noise\n" +
        "<b>Blue</b>     -> player detection zone\n" +
        "<b>Green</b>   -> vision zone\n" +
        "<b>Red</b>      -> current attack zone\n" +
        "<b>Yellow</b> -> inner/outer attack zone",
        myStyle
      );
    }
  }
}
