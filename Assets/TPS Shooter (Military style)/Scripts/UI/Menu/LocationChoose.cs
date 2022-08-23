using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using LightDev;
using LightDev.UI;

using DG.Tweening;

namespace TPSShooter.UI.Menu
{
  public class LocationChoose : CanvasElement
  {
    [Header("References")]
    public Text locationInfoText;
    public Image locationImage;

    [Header("Locations")]
    public LocationInfo[] locations;

    private int locationIndex;

    public override void Subscribe()
    {
      Events.RequestMenuLocation += Show;
    }

    public override void Unsubscribe()
    {
      Events.RequestMenuLocation -= Show;
    }

    protected override void OnStartShowing()
    {
      UpdateLocationInfo();
    }

    public void OnPlay()
    {
      Events.MenuClickSound.Call();
      Events.RequestMenuDownloading.Call(locations[locationIndex].sceneIndex);
      Hide();
    }

    public void OnBack()
    {
      Events.MenuClickSound.Call();
      Events.RequestMenuWeapon.Call();
      Hide();
    }

    public void OnNext()
    {
      locationIndex++;
      locationIndex = (locationIndex == locations.Length) ? 0 : locationIndex;
      UpdateLocationInfo();
      Events.MenuClickSound.Call();
    }

    public void OnPrevious()
    {
      locationIndex--;
      locationIndex = (locationIndex == -1) ? locations.Length - 1 : locationIndex;
      UpdateLocationInfo();
      Events.MenuClickSound.Call();
    }

    private void UpdateLocationInfo()
    {
      locationInfoText.text = locations[locationIndex].info;
      locationImage.sprite = locations[locationIndex].image;
    }

    [System.Serializable]
    public class LocationInfo
    {
      public Sprite image;
      public int sceneIndex;
      public string info;
    }
  }
}
