using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightDev.Utils
{
  public class Rotator : MonoBehaviour
  {
    public bool isLocal = true;
    public Vector3 angularVelocity;

    private void Update()
    {
      transform.Rotate(angularVelocity * Time.deltaTime, isLocal ? Space.Self : Space.World);
    }
  }
}
