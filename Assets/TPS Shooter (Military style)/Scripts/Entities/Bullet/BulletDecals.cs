using UnityEngine;

namespace TPSShooter
{
  [CreateAssetMenu]
  public class BulletDecals : ScriptableObject
  {
    public float destroyTime = 7.5f;
    public DecalProperty[] decalProperties;

    [System.Serializable]
    public class DecalProperty
    {
      public string name;
      public PhysicMaterial surfacePhysicMaterial;
      public GameObject[] decalPrefabs;
    }
  }
}
