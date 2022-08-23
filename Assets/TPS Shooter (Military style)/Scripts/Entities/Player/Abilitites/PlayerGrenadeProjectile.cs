using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LightDev;

namespace TPSShooter
{
  public class PlayerGrenadeProjectile : MonoBehaviour
  {
    public LineRenderer lineRenderer;

    private Coroutine throwCoroutine;
    private const float timeStep = 0.06f;

    private void Awake()
    {
      lineRenderer.useWorldSpace = true;
      lineRenderer.positionCount = 0;

      Events.PlayerStartGrenadeThrow += OnPlayerStartGrenadeThrow;
      Events.PlayerFinishGreandeThrow += OnPlayerFinishGreandeThrow;
    }

    private void OnDestroy()
    {
      Events.PlayerStartGrenadeThrow -= OnPlayerStartGrenadeThrow;
      Events.PlayerFinishGreandeThrow -= OnPlayerFinishGreandeThrow;
    }

    private void OnPlayerStartGrenadeThrow()
    {
      throwCoroutine = StartCoroutine(UpdateProjectile());
    }

    private void OnPlayerFinishGreandeThrow()
    {
      StopCoroutine(throwCoroutine);
      lineRenderer.positionCount = 0;
    }

    private IEnumerator UpdateProjectile()
    {
      while (true)
      {
        Vector3 startPoint = PlayerBehaviour.GetInstance().grenadeSettings.GrenadePosition.position;
        Vector3 velocity = PlayerBehaviour.GetInstance().GetGreandeVelocity();
        Vector3[] positions = CalculateGrenadePositions(startPoint, velocity);

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);

        yield return null;
      }
    }

    private Vector3[] CalculateGrenadePositions(Vector3 startPoint, Vector3 velocity)
    {
      List<Vector3> positions = new List<Vector3>() { startPoint };

      float time = timeStep;
      Vector3 nextPosition = CalculatePositionInTime(startPoint, velocity, time);
      LayerMask layers = PlayerBehaviour.GetInstance().weaponSettings.shootingLayers;
      RaycastHit hit;
      while (Physics.Linecast(positions[positions.Count - 1], nextPosition, out hit, layers) == false)
      {
        positions.Add(nextPosition);
        time += timeStep;
        nextPosition = CalculatePositionInTime(startPoint, velocity, time);
      }
      positions.Add(hit.point);

      return positions.ToArray();
    }

    private Vector3 CalculatePositionInTime(Vector3 startPoint, Vector3 velocity, float time)
    {
      startPoint.x += velocity.x * time;
      startPoint.z += velocity.z * time;
      startPoint.y += velocity.y * time + 0.5f * Physics.gravity.y * time * time;

      return startPoint;
    }
  }
}
