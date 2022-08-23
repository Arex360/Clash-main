using UnityEngine;
using UnityEngine.UI;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI.Menu
{
  public class Settings : CanvasElement
  {
    [Header("References")]
    public Toggle autoShootToggle;
    public Slider touchpadSensitivitySlider;
    public Slider touchpadAimingSensitivitySlider;

    public override void Subscribe()
    {
      Events.RequestMenuSettings += Show;
    }

    public override void Unsubscribe()
    {
      Events.RequestMenuSettings -= Show;
    }

    protected override void OnStartShowing()
    {
      autoShootToggle.isOn = SaveLoad.IsAutoShoot;
      touchpadSensitivitySlider.value = SaveLoad.TouchpadSensitivity;
      touchpadAimingSensitivitySlider.value = SaveLoad.TouchpadAimingSensitivity;
    }

    public void OnAutoshootValueChanged()
    {
      Events.MenuClickSound.Call();
      SaveLoad.IsAutoShoot = autoShootToggle.isOn;
    }

    public void OnTouchpadSensitivityChanged()
    {
      SaveLoad.TouchpadSensitivity = touchpadSensitivitySlider.value;
    }

    public void OnTouchpadAimingSensitivityChanged()
    {
      SaveLoad.TouchpadAimingSensitivity = touchpadAimingSensitivitySlider.value;
    }

    public void OnBack()
    {
      Events.MenuClickSound.Call();
      Events.RequestMenu.Call();
      Hide();
    }
  }
}
