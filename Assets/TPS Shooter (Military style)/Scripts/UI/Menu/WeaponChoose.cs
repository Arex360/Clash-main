using UnityEngine;
using UnityEngine.UI;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI.Menu
{
  public class WeaponChoose : CanvasElement
  {
    [Header("References")]
    public Text infoText;

    [Header("Weapons")]
    public GameObject[] weapons;

    private int weaponIndex;

    public override void Subscribe()
    {
      Events.RequestMenuWeapon += Show;
    }

    public override void Unsubscribe()
    {
      Events.RequestMenuWeapon -= Show;
    }

    protected override void OnStartShowing()
    {
      UpdateInfo();
    }

    public void OnBack()
    {
      Events.MenuClickSound.Call();
      Events.RequestMenu.Call();
      Hide();
    }

    public void OnPlay()
    {
      Events.MenuClickSound.Call();
      Events.RequestMenuLocation.Call();
      Hide();
    }

    public void OnNext()
    {
      weaponIndex++;
      weaponIndex = (weaponIndex == weapons.Length) ? 0 : weaponIndex;
      UpdateInfo();
      Events.MenuClickSound.Call();
    }

    public void OnPrevious()
    {
      weaponIndex--;
      weaponIndex = (weaponIndex == -1) ? weapons.Length - 1 : weaponIndex;
      UpdateInfo();
      Events.MenuClickSound.Call();
    }

    private void UpdateInfo()
    {
      infoText.text = weapons[weaponIndex].tag;
      SaveLoad.WeaponTag = weapons[weaponIndex].tag;
      for (int i = 0; i < weapons.Length; i++)
      {
        weapons[i].SetActive((i == weaponIndex) ? true : false);
      }
    }
  }
}
