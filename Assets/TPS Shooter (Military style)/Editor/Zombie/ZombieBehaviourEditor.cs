using UnityEditor;

namespace TPSShooter
{
  [CustomEditor(typeof(ZombieBehaviour))]
  [CanEditMultipleObjects]
  public class ZombieBehaviourEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      CheckWaypointsCount();
    }

    private void CheckWaypointsCount()
    {
      var waypoints = ((ZombieBehaviour)target).waypoints;
      if (waypoints != null && waypoints.Length == 1)
      {
        EditorGUILayout.HelpBox("Waypoints count must be more than 1.", MessageType.Error);
      }
    }
  }
}
