using System;
using UnityEditor;
using UnityEngine;

namespace TPSShooter
{
  public class AboutWindow : EditorWindow
  {
    private static readonly string assetName = "TPS Shooter (Military Style)";
    private static readonly string publisher = "LightDev";
    private static readonly string version = "8.0";
    private static readonly string officialSiteUrl = "https://lightdev.io/";
    private static readonly string assetStoreUrl = "https://assetstore.unity.com/packages/templates/systems/tps-shooter-military-style-99065";
    private static readonly string forumUrl = "https://forum.unity.com/threads/released-tps-shooter-military-style.568501/";
    private static readonly string email = "support@lightdev.io";

    private static readonly Vector2 windowSize = new Vector2(400, 220);
    private static readonly int offset = 100;

    [MenuItem("TPS Shooter/About", false, 0)]
    public static void Open()
    {
      AboutWindow window = ScriptableObject.CreateInstance(typeof(AboutWindow)) as AboutWindow;
      window.titleContent = new GUIContent("About");
      window.maxSize = windowSize;
      window.minSize = windowSize;
      window.ShowUtility();
    }

    protected virtual void OnGUI()
    {
      ShowAssetName();
      ShowUtility(ShowInfo);
      ShowUtility(ShowReferences);
    }

    private static void ShowAssetName()
    {
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      GUILayout.Space(15);
      GUILayout.Label(assetName, EditorStyles.AboutAssetName);
      GUILayout.Space(15);
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
    }

    private static void ShowInfo()
    {
      Label("Asset:", assetName);
      GUILayout.Space(2);
      Label("Version:", version);
      GUILayout.Space(2);
      Label("Publisher:", publisher);
    }

    private static void ShowReferences()
    {
      Button("Official Site: ", "open page", officialSiteUrl);
      GUILayout.Space(3);
      Button("AssetStore: ", "open page", assetStoreUrl);
      GUILayout.Space(3);
      Button("Forum: ", "open page", forumUrl);
      GUILayout.Space(3);
      SelectableContent("Support: ", email);
    }

    private static void ShowUtility(Action infoAction)
    {
      GUILayout.BeginVertical("HelpBox");
      GUILayout.Space(3);
      infoAction.Invoke();
      GUILayout.Space(3);
      GUILayout.EndVertical();
    }

    private static void Label(string leftText, string rightText)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(leftText, EditorStyles.AboutLabel, GUILayout.Width(offset));
      GUILayout.Label(rightText, EditorStyles.AboutLabel);
      GUILayout.EndHorizontal();
    }

    private static void SelectableContent(string leftText, string rightText)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(leftText, EditorStyles.AboutLabel, GUILayout.Width(offset));
      EditorGUILayout.SelectableLabel(rightText, EditorStyles.AboutLink);
      GUILayout.EndHorizontal();
    }

    private static void Button(string leftText, string rightText, string url)
    {
      GUILayout.BeginHorizontal(GUILayout.Height(1));
      GUILayout.Label(leftText, EditorStyles.AboutLabel, GUILayout.Width(offset));
      if (GUILayout.Button(rightText, EditorStyles.AboutLink))
      {
        Application.OpenURL(url);
      }
      GUILayout.EndHorizontal();
    }
  }
}