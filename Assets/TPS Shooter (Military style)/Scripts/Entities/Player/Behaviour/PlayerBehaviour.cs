using UnityEngine;
using System;
using LightDev;
using LightDev.Core;

namespace TPSShooter
{
  [RequireComponent(typeof(FootstepSounds))]
  [RequireComponent(typeof(CharacterController))]
  [RequireComponent(typeof(Animator))]
  public partial class PlayerBehaviour : Base
  {
    public PlayerWeaponSettings weaponSettings = new PlayerWeaponSettings();
    public PlayerGrenadeSettings grenadeSettings = new PlayerGrenadeSettings();
    public PlayerSounds sounds = new PlayerSounds();
    public PlayerSettingsIK IkSettings = new PlayerSettingsIK();
    public PlayerCrouchSettings crouchSettings = new PlayerCrouchSettings();
    public PlayerMovementSettings movementSettings = new PlayerMovementSettings();
    private PlayerAnimationParameters animationsParameters = new PlayerAnimationParameters();

    private CharacterController _characterController;
    private Animator _animator;

    private static PlayerBehaviour instance;

    public bool IsAlive { get; private set; } = true;
    public float Noise { get { return GetNoise(); } }
    public static PlayerBehaviour GetInstance() { return instance; }
    private bool isSubsribed;
    #region MonoBehaviour

    private void OnValidate()
    {
      crouchSettings.CharacterHeightCrouching = Mathf.Clamp(crouchSettings.CharacterHeightCrouching, 0, GetComponent<CharacterController>().height);
      crouchSettings.CharacterCenterCrouching.y = Mathf.Max(crouchSettings.CharacterCenterCrouching.y, 0);

      movementSettings.AirSpeed = Mathf.Max(movementSettings.AirSpeed, 0);
      movementSettings.JumpSpeed = Mathf.Max(movementSettings.JumpSpeed, 0);
      movementSettings.JumpTime = Mathf.Max(movementSettings.JumpTime, 0);
      movementSettings.ForwardSpeed = Mathf.Max(movementSettings.ForwardSpeed, 0);
      movementSettings.StrafeSpeed = Mathf.Max(movementSettings.StrafeSpeed, 0);
      movementSettings.SprintSpeed = Mathf.Max(movementSettings.SprintSpeed, 0);
      movementSettings.CrouchForwardSpeed = Mathf.Max(movementSettings.CrouchForwardSpeed, 0);
      movementSettings.CrouchStrafeSpeed = Mathf.Max(movementSettings.CrouchStrafeSpeed, 0);

      ValidateWeapons();
    }

    private void Awake()
    {
      instance = this;

      _characterController = GetComponent<CharacterController>();
      _animator = GetComponent<Animator>();
      _animator.applyRootMotion = movementSettings.ApplyRootMotion;

      CheckLayers();

      InitializeWeapon();
      InitializeCrouch();

      Subscribe();
    }

    private void OnDestroy()
    {
      Unsubscribe();
    }

    private void Update()
    {
      if(!isSubsribed) return;
      UpdateGroundCheck();

      UpdateWalk();
      UpdateRun();

      UpdateGravity();
      UpdateMovementSpeed();

      UpdateFirePoint();
    }

    private void LateUpdate()
    {
      if(!isSubsribed) return;
      UpdateSpineIK();
    }

    private void OnAnimatorIK(int layerIndex)
    {
      if(!isSubsribed) return;
      UpdateLeftHandIK();
    }

    #endregion

    private void CheckLayers()
    {
      if (!LayerMask.LayerToName(gameObject.layer).Equals(Layers.Player))
        Debug.LogError("PlayerBehaviour: Player has to be layered as Player.");
    }

    private void Subscribe()
    {
      Events.JumpRequested += OnJumpRequested;
      Events.CrouchRequested += OnCrouchRequested;
      Events.FireRequested += OnFireRequested;
      Events.ReloadRequested += OnReloadRequested;
      Events.SwapWeaponRequested += OnSwapWeaponRequested;
      Events.DropWeaponRequested += OnDropWeaponRequested;

      Events.GrenadeStartThrowRequest += OnGrenadeStartThrowRequest;
      Events.GrenadeFinishThrowRequest += OnGrenadeFinishThrowRequest;

      Events.AimActivateRequested += OnAimActivateRequested;

      Events.PlayerGetInVehicle += OnPlayerGetInVehicle;
      Events.PlayerGetOutVehicle += OnPlayerGetOutVehicle;
      isSubsribed = true;
    }

    public void Unsubscribe()
    {
      isSubsribed = false;
      Events.JumpRequested -= OnJumpRequested;
      Events.CrouchRequested -= OnCrouchRequested;

      Events.FireRequested -= OnFireRequested;
      Events.ReloadRequested -= OnReloadRequested;
      Events.SwapWeaponRequested -= OnSwapWeaponRequested;
      Events.DropWeaponRequested -= OnDropWeaponRequested;

      Events.GrenadeStartThrowRequest -= OnGrenadeStartThrowRequest;
      Events.GrenadeFinishThrowRequest -= OnGrenadeFinishThrowRequest;

      Events.AimActivateRequested -= OnAimActivateRequested;

      Events.PlayerGetInVehicle -= OnPlayerGetInVehicle;
      Events.PlayerGetOutVehicle -= OnPlayerGetOutVehicle;
    }

    private float GetNoise()
    {
      if (IsDrivingVehicle) return 30;
      if (IsFire) return 30;
      if (_jumpingTriggered) return 7;
      if (_animator.GetFloat(animationsParameters.verticalMovementFloat) != 0 || _animator.GetFloat(animationsParameters.horizontalMovementFloat) != 0)
      {
        return IsCrouching ? 3 : 5;
      }
      if (IsReloading) return 3;

      return 0;
    }
    public void EnableWeapon(int id){
      for(int i = 0; i<weaponSettings.AllWeapons.Length;i++){
        print(weaponSettings.AllWeapons[i].name);
        weaponSettings.AllWeapons[i].gameObject.SetActive(false);
      }
      weaponSettings.AllWeapons[id].gameObject.SetActive(true);
    }
    public void Die()
    {
      if (!IsAlive) return;

      IsAlive = false;

      _characterController.enabled = false;

      // play die animation if the player is not in a car
      if (!IsDrivingVehicle)
        _animator.SetTrigger(animationsParameters.dieTrigger);

      _animator.SetBool(animationsParameters.aimingBool, false);

      if (IsThrowingGrenade)
        Events.GrenadeFinishThrowRequest.Call();

      if (IsAiming)
        DeactivateAiming();

      Events.PlayerDied.Call();
    }
  }
}
