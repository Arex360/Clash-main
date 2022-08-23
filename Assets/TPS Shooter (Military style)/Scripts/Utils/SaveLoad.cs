using UnityEngine;

namespace TPSShooter
{
  public static class SaveLoad
  {
    private const string WeaponTagKey = "Weapon";
    private const string TouchpadSensitivityKey = "TouchS";
    private const string TouchpadAimingSensitivityKey = "TouchAS";
    private const string IsAutoShootKey = "AutoShot";

    public static string WeaponTag
    {
      get { return PlayerPrefs.GetString(WeaponTagKey, ""); }
      set { PlayerPrefs.SetString(WeaponTagKey, value); }
    }

    public static float TouchpadSensitivity
    {
      get { return PlayerPrefs.GetFloat(TouchpadSensitivityKey, 30); }
      set { PlayerPrefs.SetFloat(TouchpadSensitivityKey, value); }
    }

    public static float TouchpadAimingSensitivity
    {
      get { return PlayerPrefs.GetFloat(TouchpadAimingSensitivityKey, 14); }
      set { PlayerPrefs.SetFloat(TouchpadAimingSensitivityKey, value); }
    }

    public static bool IsAutoShoot
    {
      get { return PlayerPrefs.GetInt(IsAutoShootKey, 0) == 1; }
      set { PlayerPrefs.SetInt(IsAutoShootKey, (value == true) ? 1 : 0); }
    }
  }
}
