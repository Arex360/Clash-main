using UnityEngine;

using LightDev;

namespace TPSShooter
{
  [RequireComponent(typeof(PlayerBehaviour))]
  public class PlayerVehicleAbility : MonoBehaviour
  {
    public TPSCamera tpsCamera;
    public bool hideSkinWhileDriving = true;
    public GameObject skin;

    private bool _isDriving;
    private bool _isCarDetected;

    private void Awake()
    {
      Events.UseVehicleRequested += OnUseVehicleRequested;
    }

    private void OnDestroy()
    {
      Events.UseVehicleRequested -= OnUseVehicleRequested;
    }

    private void Update()
    {
      if (_isDriving)
      {
        UpdatePlayerPositionInVehicle();
      }
      else
      {
        CheckVehicleDetection();
      }
    }

    private void OnUseVehicleRequested()
    {
      if (_isDriving)
      {
        GetOutVehicle();
      }
      else if (_isCarDetected)
      {
        GetInVehicle();
      }
    }

    private void CheckVehicleDetection()
    {
      // Check nearby vehicles
      if (CheckNearbyVehicles())
      {
        if (!_isCarDetected)
        {
          _isCarDetected = true;
          Events.PlayerDetectVehicle.Call();
        }
      }
      else
      {
        if (_isCarDetected)
        {
          _isCarDetected = false;
          Events.PlayerUndetectVehicle.Call();
        }
      }
    }

    // The minimum distance between car and the player, when the player can get in car.
    private readonly float minVehicleDistance = 3f;
    private Vehicle currentVechicleBehaviour;

    // Checks if there are any vehicle that the player can use.
    private bool CheckNearbyVehicles()
    {
      if (PlayerBehaviour.GetInstance().FireHitObject == null) return false;
      if (!PlayerBehaviour.GetInstance().FireHitObject.GetComponentInParent<Vehicle>()) return false;
      if (Vector3.Distance(transform.position, PlayerBehaviour.GetInstance().FireHitObject.position) > minVehicleDistance) return false;

      VehicleHealthBar vehicleHP = PlayerBehaviour.GetInstance().FireHitObject.GetComponent<VehicleHealthBar>();
      if(vehicleHP && vehicleHP.WasExplode) return false;

      return true;
    }

    // The player gets the in car.
    private void GetInVehicle()
    {
      // Determines a new VehicleBehaviour
      currentVechicleBehaviour = PlayerBehaviour.GetInstance().FireHitObject.GetComponent<Vehicle>();

      // sets the player state
      _isDriving = true;

      // calls this method in order to vehicle make some actions
      currentVechicleBehaviour.PlayerGetIn();

      // makes the player invisible
      if (hideSkinWhileDriving)
        skin.SetActive(false);

      PlayerBehaviour.GetInstance().IsDrivingVehicle = true;
      PlayerBehaviour.GetInstance().DrivingVehicle = currentVechicleBehaviour;
      Events.PlayerGetInVehicle.Call();
    }

    // The plyaer gets the out car.
    private void GetOutVehicle()
    {
      // calls this method in order to vehicle make some actions
      currentVechicleBehaviour.PlayerGetOut();

      // sets the player's state
      _isDriving = false;

      // sets position on the ground
      transform.position = new Vector3(
          currentVechicleBehaviour.playerStand.position.x,
          currentVechicleBehaviour.playerStand.position.y + 1.5f,
          currentVechicleBehaviour.playerStand.position.z
      );

      transform.rotation = Quaternion.Euler(0, currentVechicleBehaviour.playerStand.rotation.y, 0);

      // makes the player's skin visible
      if (hideSkinWhileDriving)
        skin.SetActive(true);

      PlayerBehaviour.GetInstance().IsDrivingVehicle = false;
      PlayerBehaviour.GetInstance().DrivingVehicle = null;
      Events.PlayerGetOutVehicle.Call();
    }

    // Updates the player position in vehicle.
    private void UpdatePlayerPositionInVehicle()
    {
      transform.position = currentVechicleBehaviour.playerSit.position;
      transform.rotation = currentVechicleBehaviour.playerSit.rotation;
    }
  }
}
