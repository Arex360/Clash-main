using UnityEngine;

using LightDev;
using LightDev.Core;

using DG.Tweening;

namespace TPSShooter
{
  public class Helicopter : Base
  {
    [Header("Rotor")]
    public Transform rotor;

    [Header("Sound")]
    public AudioSource motorSound;

    [Header("Movement")]
    public CheckPoint[] points;

    private CheckPoint nextPoint;
    private int nextPointIndex = 0;
    private float rotorSpeed = 440;

    private void Awake()
    {
      if(points.Length == 1)
      {
        Debug.LogError("Helicopter: points count must be more than 1.");
      }

      Events.GamePaused += motorSound.Stop;
      Events.GameResumed += motorSound.Play;

      UpdatePointsRotation();
      SetupPosition();
      UpdateNextPoint();
      Move();
    }

    private void OnDestroy()
    {
      Events.GamePaused -= motorSound.Stop;
      Events.GameResumed -= motorSound.Play;
    }

    private void UpdatePointsRotation()
    {
      for(int i = 0; i < points.Length; i++)
      {
        points[i % points.Length].point.LookAt(points[(i + 1) % points.Length].point);
      }
    }

    private void SetupPosition()
    {
      if(points.Length < 2) return;

      SetPosition(points[0].point.position);
      SetRotation(points[0].point.rotation);
    }

    private void UpdateNextPoint()
    {
      if(points.Length < 2) return;
      
      nextPointIndex++;
      nextPoint = points[nextPointIndex % points.Length];
    }

    private void Move()
    {
      if(points.Length < 2) return;

      Sequence(
        Move(nextPoint.point.position, nextPoint.moveTime).SetEase(Ease.InOutQuad),
        OnFinish(() => UpdateNextPoint()),
        Rotate(nextPoint.point.rotation, 2).SetEase(Ease.InOutQuad),
        OnFinish(() => Move())
      );
    }

    private void RotateRotor()
    {
      rotor?.RotateAround(rotor.position, rotor.up, rotorSpeed * Time.deltaTime);
    }

    private void Update()
    {
      RotateRotor();
    }

    [System.Serializable]
    public class CheckPoint
    {
      public Transform point;
      public float moveTime;
    }
  }
}
