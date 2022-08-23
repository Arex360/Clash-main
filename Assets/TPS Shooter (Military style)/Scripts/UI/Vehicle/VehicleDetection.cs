using UnityEngine;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class VehicleDetection : CanvasElement
  {
    public GameObject getInVehichleUI;
    public GameObject getOutVehicleUI;
    
    public override void Subscribe()
    {
      base.Subscribe();

      Events.SceneLoaded += Show;
      Events.PlayerDetectVehicle += OnPlayerDetectVehicle;
      Events.PlayerUndetectVehicle += OnPlayerUndetectVehicle;
      Events.PlayerGetInVehicle += OnPlayerGetInVehicle;
      Events.PlayerGetOutVehicle += OnPlayerGetOutVehicle;
      Events.GameFinished += Hide;
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

      Events.SceneLoaded -= Show;
      Events.PlayerDetectVehicle -= OnPlayerDetectVehicle;
      Events.PlayerUndetectVehicle -= OnPlayerUndetectVehicle;
      Events.PlayerGetInVehicle -= OnPlayerGetInVehicle;
      Events.PlayerGetOutVehicle -= OnPlayerGetOutVehicle;
      Events.GameFinished -= Hide;
    }

    protected override void OnStartShowing()
    {
      base.OnStartShowing();

      getInVehichleUI.SetActive(false);
      getOutVehicleUI.SetActive(false);
    }

    private void OnPlayerDetectVehicle()
    {
      getInVehichleUI.SetActive(true);
    }

    private void OnPlayerUndetectVehicle()
    {
      getInVehichleUI.SetActive(false);
    }

    private void OnPlayerGetInVehicle()
    {
      getInVehichleUI.SetActive(false);
      getOutVehicleUI.SetActive(true);
    }

    private void OnPlayerGetOutVehicle()
    {
      getOutVehicleUI.SetActive(false);
    }

    public void OnUseVehicleClick() { Events.UseVehicleRequested.Call(); }
  }
}
