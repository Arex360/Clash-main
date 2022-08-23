using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Readme))]
[InitializeOnLoad]
public class ReadmeEditor : Editor
{
  protected bool isStylesInitialized;
  protected GUIStyle linkStyle;
  protected GUIStyle titleStyle;
  protected GUIStyle headingStyle;
  protected GUIStyle bodyStyle;

  protected const string isReadmeShownKey = "Readme.isShown";
  protected const float spaceBetweenSections = 16f;

  static ReadmeEditor()
  {
    EditorApplication.delayCall += SelectReadmeOnLoad;
  }

  protected static void SelectReadmeOnLoad()
  {
    if (!SessionState.GetBool(isReadmeShownKey, false))
    {
      SelectReadme();
      SessionState.SetBool(isReadmeShownKey, true);
    }
  }

  protected static void SelectReadme()
  {
    string[] ids = AssetDatabase.FindAssets("t:" + typeof(Readme).Name);
    Object readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
    Selection.objects = new UnityEngine.Object[] { readmeObject };
  }

  protected override void OnHeaderGUI()
  {
    InitializeStyles();

    Readme readme = (Readme)target;
    float iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

    GUILayout.BeginHorizontal("In BigTitle");
    GUILayout.Label(readme.header.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
    GUILayout.Label(readme.header.title, titleStyle);
    GUILayout.EndHorizontal();
  }

  public override void OnInspectorGUI()
  {
    InitializeStyles();

    var readme = (Readme)target;
    foreach (var section in readme.sections)
    {
      if (!string.IsNullOrEmpty(section.heading))
      {
        GUILayout.Label(section.heading, headingStyle);
      }
      if (!string.IsNullOrEmpty(section.text))
      {
        GUILayout.Label(section.text, bodyStyle);
      }
      if (!string.IsNullOrEmpty(section.linkText))
      {
        if (LinkLabel(new GUIContent(section.linkText)))
        {
          Application.OpenURL(section.url);
        }
      }
      GUILayout.Space(spaceBetweenSections);
    }
  }

  protected void InitializeStyles()
  {
    if (isStylesInitialized) return;

    bodyStyle = new GUIStyle(EditorStyles.label);
    bodyStyle.wordWrap = true;
    bodyStyle.fontSize = 14;

    titleStyle = new GUIStyle(bodyStyle);
    titleStyle.fontSize = 26;

    headingStyle = new GUIStyle(bodyStyle);
    headingStyle.fontSize = 18;

    linkStyle = new GUIStyle(bodyStyle);
    linkStyle.wordWrap = false;
    linkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
    linkStyle.stretchWidth = false;

    isStylesInitialized = true;
  }

  protected bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
  {
    Rect position = GUILayoutUtility.GetRect(label, linkStyle, options);

    Handles.BeginGUI();
    Handles.color = linkStyle.normal.textColor;
    Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
    Handles.color = Color.white;
    Handles.EndGUI();

    EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

    return GUI.Button(position, label, linkStyle);
  }
}

