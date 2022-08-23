using UnityEditor;
using UnityEngine;

namespace TPSShooter
{
  public class EditorStyles
  {
    public static GUIStyle AboutAssetName
    {
      get
      {
        GUIStyle style = new GUIStyle();
        style.fontSize = 22;
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = new Color32(50, 55, 60, 255);
        return style;
      }
    }

    public static GUIStyle AboutLabel
    {
      get
      {
        GUIStyle style = new GUIStyle();
        style.fontSize = 13;
        style.fontStyle = FontStyle.Bold;
        return style;
      }
    }

    public static GUIStyle AboutLink
    {
      get
      {
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 13;
        style.normal.textColor = new Color32(0, 70, 255, 255);
        return style;
      }
    }

    private GUIStyle _headerStyle;
    private GUIStyle _labelStyle;

    public delegate void ShowGUI();

    public EditorStyles()
    {
      SetupHeaderStyle();
      SetupLabelStyle();
    }

    private void SetupHeaderStyle()
    {
      _headerStyle = new GUIStyle();
      _headerStyle.normal.textColor = Color.black;
      _headerStyle.fontStyle = FontStyle.Bold;
      _headerStyle.fontSize = 18;
      _headerStyle.alignment = TextAnchor.MiddleCenter;
    }

    private void SetupLabelStyle()
    {
      _labelStyle = new GUIStyle();
      _labelStyle.normal.textColor = Color.black;
      _labelStyle.fontStyle = FontStyle.Bold;
      _labelStyle.fontSize = 12;
      _labelStyle.alignment = TextAnchor.MiddleCenter;
    }

    public void ShowWrapperGUI(ShowGUI showGUI)
    {
      AddDoubleSpace();

      EditorGUILayout.BeginVertical("Box");
      showGUI();
      EditorGUILayout.EndVertical();
    }

    public void ShowLabelInfo(string text)
    {
      EditorGUILayout.LabelField(text, _labelStyle);
    }

    public void ShowHeaderInfo(string text)
    {
      EditorGUILayout.LabelField(text, _headerStyle);
    }

    public void ShowErrorHelpBox(string text)
    {
      EditorGUILayout.HelpBox(text, MessageType.Error);
    }

    public void AddDoubleSpace()
    {
      EditorGUILayout.Space();
      EditorGUILayout.Space();
    }
  }
}
