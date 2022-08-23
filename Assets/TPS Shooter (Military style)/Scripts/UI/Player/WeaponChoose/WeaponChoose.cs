using UnityEngine;

using LightDev.UI;

namespace TPSShooter.UI
{
  public class WeaponChoose : CanvasElement
  {
    public WeaponChooseButton[] buttons;
    public WeaponIcons weaponIcons;

    protected int selectedButtonId;

    public override void Subscribe()
    {
      foreach (WeaponChooseButton button in buttons)
      {
        button.onSelected += OnButtonSelected;
        button.onDeselected += OnButtonDeselected;
      }
    }

    public override void Unsubscribe()
    {
      foreach (WeaponChooseButton button in buttons)
      {
        button.onSelected -= OnButtonSelected;
        button.onDeselected -= OnButtonDeselected;
      }
    }

    protected override void OnStartShowing()
    {
      selectedButtonId = -1;
      UpdateButtonsInfo();
    }

    protected virtual void UpdateButtonsInfo()
    {
      PlayerWeapon[] playerWeapons = PlayerBehaviour.GetInstance().weaponSettings.AllWeapons;
      for (int i = 0; i < buttons.Length; i++)
      {
        var button = buttons[i];
        string info = "";
        Sprite weaponSprite = null;
        int buttonID = -1;
        if (i < playerWeapons.Length && playerWeapons[i] != null)
        {
          var weapon = playerWeapons[i];
          info = (weapon.BulletsAmount + weapon.BulletsInMag).ToString();
          weaponSprite = weaponIcons.GetIconByGameObjectTag(weapon.gameObject.tag);
          buttonID = i;
        }
        button.SetInfo(info);
        button.SetWeaponIcon(weaponSprite);
        button.buttonID = buttonID;
      }
    }

    public void OnButtonSelected(int buttonID)
    {
      selectedButtonId = buttonID;
    }

    public void OnButtonDeselected(int weaponID)
    {
      selectedButtonId = -1;
    }
  }
}
