using UnityEngine;

namespace TPSShooter
{
  public class DesiredFrameRate : MonoBehaviour
  {
    public int frameRate = 60;

    private void Awake()
    {
      Application.targetFrameRate = frameRate;
    }
  }
}
