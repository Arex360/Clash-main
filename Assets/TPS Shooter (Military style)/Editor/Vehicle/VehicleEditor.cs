using UnityEngine;
using UnityEditor;

namespace TPSShooter
{
  [CustomEditor(typeof(Vehicle))]
  [CanEditMultipleObjects]
  public class VehicleEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      CheckLayer(Layers.Vehicle);
    }

    private void CheckLayer(string layerName)
    {
      if (LayerMask.NameToLayer(layerName) != ((Vehicle)target).gameObject.layer)
      {
        EditorGUILayout.HelpBox($"GameObject must have {layerName} layer.", MessageType.Error);
      }
    }
  }
}
