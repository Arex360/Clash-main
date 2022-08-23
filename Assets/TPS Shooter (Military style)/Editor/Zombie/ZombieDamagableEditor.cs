using UnityEngine;
using UnityEditor;

namespace TPSShooter
{
  [CustomEditor(typeof(ZombieDamagable))]
  [CanEditMultipleObjects]
  public class ZombieDamagableEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      CheckLayer(Layers.EnemyDamagable);
    }

    private void CheckLayer(string layerName)
    {
      if (LayerMask.NameToLayer(layerName) != ((ZombieDamagable)target).gameObject.layer)
      {
        EditorGUILayout.HelpBox($"GameObject must have {layerName} layer.", MessageType.Error);
      }
    }
  }
}
