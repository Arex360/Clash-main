using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TPSShooter
{
  public class TPSEditor
  {
    private const string desktopInputPrefabPath = "Assets/TPS Shooter (Military style)/Prefabs/Input/DesktopInput.prefab";
    private const string mobileInputPrefabPath = "Assets/TPS Shooter (Military style)/Prefabs/Input/MobileInput.prefab";
    private const string gameManagerPrefabPath = "Assets/TPS Shooter (Military style)/Prefabs/Entities/GameManager.prefab";
    private const string playerCanvasPrefabPath = "Assets/TPS Shooter (Military style)/Prefabs/UI/Player/PlayerCanvas.prefab";

    [MenuItem("TPS Shooter/Select/Bullet Decals", false, 50)]
    private static void SelectDecals()
    {
      SelectUtil(nameof(BulletDecals));
    }

    [MenuItem("TPS Shooter/Select/Footstep Data", false, 51)]
    private static void SelectFootstep()
    {
      SelectUtil(nameof(FootsetpData));
    }

    [MenuItem("TPS Shooter/Select/Weapon Icons", false, 52)]
    private static void SelectWeaponIcons()
    {
      SelectUtil(nameof(WeaponIcons));
    }

    [MenuItem("TPS Shooter/Add to scene/Desktop Input", false, 20)]
    private static void AddDesktopInputToScene()
    {
      var obj = AssetDatabase.LoadAssetAtPath(desktopInputPrefabPath, typeof(GameObject));
      GameObject.Instantiate(obj).name = "DesktopInput";

      TryAddEventSystem();
    }

    [MenuItem("TPS Shooter/Add to scene/Mobile Input", false, 21)]
    private static void AddMobileInputToScene()
    {
      var obj = AssetDatabase.LoadAssetAtPath(mobileInputPrefabPath, typeof(GameObject));
      GameObject.Instantiate(obj).name = "MobileInput";

      TryAddEventSystem();
    }

    [MenuItem("TPS Shooter/Add to scene/Game Manager", false, 21)]
    private static void AddGameManagerToScene()
    {
      var obj = AssetDatabase.LoadAssetAtPath(gameManagerPrefabPath, typeof(GameObject));
      GameObject.Instantiate(obj).name = "GameManager";

      TryAddEventSystem();
    }

    [MenuItem("TPS Shooter/Add to scene/Player UI", false, 22)]
    private static void AddPlayerCanvasToScene()
    {
      var obj = AssetDatabase.LoadAssetAtPath(playerCanvasPrefabPath, typeof(GameObject));
      GameObject.Instantiate(obj).name = "PlayerCanvas";
    }

    [MenuItem("TPS Shooter/Clear Saved Data (PlayerPrefs)", false, 500)]
    private static void ClearAllData()
    {
      PlayerPrefs.DeleteAll();
    }

    private static void SelectUtil(string className)
    {
      Selection.activeInstanceID = GetObjectByClassName(className).GetInstanceID();
    }

    private static UnityEngine.Object GetObjectByClassName(string className)
    {
      foreach (string guid in AssetDatabase.FindAssets($"t:{className}"))
      {
        return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(guid));
      }

      return null;
    }

    private static void TryAddEventSystem()
    {
      if (GameObject.FindObjectOfType<EventSystem>()) return;

      var obj = new GameObject("EventSystem");
      obj.AddComponent(typeof(EventSystem));
      obj.AddComponent(typeof(StandaloneInputModule));
    }
  }
}
