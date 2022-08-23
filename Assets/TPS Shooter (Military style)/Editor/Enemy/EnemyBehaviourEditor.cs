using UnityEditor;

namespace TPSShooter
{
  [CustomEditor(typeof(EnemyBehaviour))]
  [CanEditMultipleObjects]
  public class EnemyBehaviourEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      CheckWaypointsCount();
    }

    private void CheckWaypointsCount()
    {
      int waypointsCount = ((EnemyBehaviour)target).PatrollingSettings.Waypoints.Length;
      if (waypointsCount == 1)
      {
        EditorGUILayout.HelpBox("PatrollingSettings: Waypoints count must be more than 1.", MessageType.Error);
      }
    }
  }
}
