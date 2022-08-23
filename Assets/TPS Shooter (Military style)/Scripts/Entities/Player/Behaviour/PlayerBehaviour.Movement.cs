using System.Collections;
using UnityEngine;

using LightDev;

namespace TPSShooter
{
  public partial class PlayerBehaviour
  {
    public bool IsRunning { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsGrounded { get; private set; }

    private float _characterControllerInitialHeight;
    private Vector3 _characterControllerInitialCenter;

    private void InitializeCrouch()
    {
      _characterControllerInitialHeight = _characterController.height;
      _characterControllerInitialCenter = _characterController.center;
    }

    private void OnJumpRequested()
    {
      if (!IsAlive) return;
      if (IsDrivingVehicle) return;

      if (IsCrouching)
        Stand();
      else
      {
        if (IsAiming)
          DeactivateAiming();
        CharacterControllerJump();
      }
    }

    private void OnCrouchRequested()
    {
      if (!IsAlive) return;
      if (IsDrivingVehicle) return;

      if (IsCrouching)
        Stand();
      else
        Crouch();
    }

    private void UpdateGroundCheck()
    {
      if (IsAlive == false) return;
      if (IsDrivingVehicle) return;

      IsGrounded = CheckGrounded();
      _animator.SetBool(animationsParameters.groundedBool, IsGrounded);

      _animator.SetBool(animationsParameters.jumpBool, IsJumping);
    }

    private float _forward;
    private float _strafe;
    private readonly float _movementLerpSpeed = 15f;

    private void UpdateWalk()
    {
      if (IsAlive == false) return;
      if (IsDrivingVehicle) return;

      _forward = Mathf.MoveTowards(_forward, InputController.VerticalMovement, _movementLerpSpeed * Time.deltaTime);
      _strafe = Mathf.MoveTowards(_strafe, InputController.HorizontalMovement, _movementLerpSpeed * Time.deltaTime);

      _animator.SetFloat(animationsParameters.verticalMovementFloat, _forward);
      _animator.SetFloat(animationsParameters.horizontalMovementFloat, _strafe);
    }

    private void UpdateRun()
    {
      if (IsAlive == false) return;
      if (IsDrivingVehicle) return;

      if (InputController.IsRun &&
          ((!IsUnarmedMode && _forward > 0.3f) || (IsUnarmedMode)) &&
          !IsReloading &&
          !IsThrowingGrenade &&
          !IsFire &&
          !IsJumping &&
          !IsCrouching &&
          !IsAiming
      )
        IsRunning = true;
      else
        IsRunning = false;

      _animator.SetBool(animationsParameters.runBool, IsRunning);
    }

    private void UpdateMovementSpeed()
    {
      if (IsAlive == false) return;
      if (IsDrivingVehicle) return;

      Vector3 movement = new Vector3(_strafe, 0, _forward);
      movement.Normalize();

      if (IsGrounded)
      {
        if (movementSettings.ApplyRootMotion)
        {
          movement = Vector3.zero;
        }
        else
        {
          if (IsCrouching)
          {
            movement.x *= movementSettings.CrouchStrafeSpeed;
            movement.z *= movementSettings.CrouchForwardSpeed;
          }
          else
          {
            movement.z *= IsRunning ? movementSettings.SprintSpeed : movementSettings.ForwardSpeed;
            movement.x *= movementSettings.StrafeSpeed;
          }
        }
      }
      else
      {
        movement *= movementSettings.AirSpeed;
      }

      movement = transform.TransformDirection(movement);
      _characterController.Move(movement * Time.deltaTime);
    }

    private bool _resetGravity;
    private float _gravity;

    private float _gravityModifier = 9.81f;
    private float _baseGravity = 50.0f;
    private float _resetGravityValue = 1.2f;

    private void UpdateGravity()
    {
      if (IsAlive == false) return;
      if (IsDrivingVehicle) return;

      if (!IsGrounded)
      {
        if (!_resetGravity)
        {
          _gravity = _resetGravityValue;
          _resetGravity = true;
        }
        _gravity += Time.deltaTime * _gravityModifier;
      }
      else
      {
        _gravity = _baseGravity;
        _resetGravity = false;
      }

      Vector3 gravityVector = new Vector3();

      if (_jumpingTriggered)
        gravityVector.y = movementSettings.JumpSpeed;
      else
        gravityVector.y -= _gravity;

      _characterController.Move(gravityVector * Time.deltaTime);
    }

    private bool CheckGrounded()
    {
      RaycastHit hit;
      Vector3 start = transform.position + transform.up;
      Vector3 dir = Vector3.down;
      float radius = _characterController.radius;
      if (Physics.SphereCast(start, radius, dir, out hit, _characterController.height * 0.6f))
        return true;

      return false;
    }

    private bool _jumpingTriggered;

    // Makes the character jump
    private void CharacterControllerJump()
    {
      if (_jumpingTriggered)
        return;

      if (IsGrounded)
      {
        sounds.PlaySound(sounds.JumpSound);

        _jumpingTriggered = true;
        StartCoroutine(StopJump());
      }
    }

    // Stops us from jumping
    private IEnumerator StopJump()
    {
      yield return new WaitForSeconds(movementSettings.JumpTime);
      _jumpingTriggered = false;
    }

    // Animation event
    private void StartJumping()
    {
      IsJumping = true;
    }

    // Animation event
    private void FinishedJumping()
    {
      IsJumping = false;
    }

    // Animation event
    private void LandSound()
    {
      sounds.PlaySound(sounds.LandSound);
    }

    private void Crouch()
    {
      IsCrouching = true;
      _animator.SetBool(animationsParameters.crouchBool, IsCrouching);

      _characterController.center = crouchSettings.CharacterCenterCrouching;
      _characterController.height = crouchSettings.CharacterHeightCrouching;

      Events.PlayerCrouch.Call();
    }

    private void Stand()
    {
      IsCrouching = false;
      _animator.SetBool(animationsParameters.crouchBool, IsCrouching);

      _characterController.center = _characterControllerInitialCenter;
      _characterController.height = _characterControllerInitialHeight;

      Events.PlayerStand.Call();
    }
  }
}
