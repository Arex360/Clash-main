using UnityEngine;

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
      UpdateGroundCheck();

      UpdateWalk();
      UpdateRun();

      UpdateGravity();
      UpdateMovementSpeed();

      UpdateFirePoint();
    }

    private void LateUpdate()
    {
      UpdateSpineIK();
    }

    private void OnAnimatorIK(int layerIndex)
    {
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
    }

    private void Unsubscribe()
    {
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
