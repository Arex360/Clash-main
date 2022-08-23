using UnityEngine;

namespace TPSShooter
{
  [CreateAssetMenu(fileName = "WeaponIcons", menuName = "WeaponIcons", order = 0)]
  public class WeaponIcons : ScriptableObject
  {
    public WeaponIconInfo[] weaponIconInfos;

    public Sprite GetIconByGameObjectTag(string tag)
    {
      for (int i = 0; i < weaponIconInfos.Length; i++)
      {
        if (weaponIconInfos[i].gameObjectTag.Equals(tag))
        {
          return weaponIconInfos[i].weaponIcon;
        }
      }

      return null;
    }

    [System.Serializable]
    public class WeaponIconInfo
    {
      public Sprite weaponIcon;
      public string gameObjectTag;
    }
  }
}
