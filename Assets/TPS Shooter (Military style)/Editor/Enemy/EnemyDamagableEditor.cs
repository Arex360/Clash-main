using UnityEngine;
using UnityEditor;

namespace TPSShooter
{
  [CustomEditor(typeof(EnemyDamagable))]
  [CanEditMultipleObjects]
  public class EnemyDamagableEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      CheckLayer(Layers.EnemyDamagable);
    }

    private void CheckLayer(string layerName)
    {
      if (LayerMask.NameToLayer(layerName) != ((EnemyDamagable)target).gameObject.layer)
      {
        EditorGUILayout.HelpBox($"GameObject must have {layerName} layer.", MessageType.Error);
      }
    }
  }
}
