using UnityEngine;
using UnityEngine.UI;

namespace TPSShooter
{
  public static class CustomExtensions
  {
    public static float WrapAngle(this float angle)
    {
      angle %= 360;
      if (angle > 180)
        return angle - 360;

      return angle;
    }

    public static void SetFade(this Text text, float value)
    {
      Color color = text.color;
      color.a = value;
      text.color = color;
    }

    public static void AnulateRotationExceptY(this Transform transform)
    {
      Vector3 rotation = transform.eulerAngles;
      rotation.x = 0;
      rotation.z = 0;
      transform.eulerAngles = rotation;
    }

    public static Color SetAlpha(this Color color, float alpha)
    {
      color.a = alpha;
      return color;
    }
  }
}
