using System.Collections;
using UnityEngine;

using LightDev.Core;

namespace TPSShooter
{
  public class PlayerWeapon : Base
  {
    [Header("- Bullet settings -")]
    public GameObject BulletPrefab;
    public Transform BulletPosition;

    [Header("- Weapon settings -")]
    public int MagCapacity = 30;
    public int BulletsInMag = 30;
    public int BulletsAmount = 30;
    public float ShootFrequency = 0.14f;

    [Header("- Recoil -"), Range(0, 4f)]
    public float MinDeviationAlongY = 0.1f;
    [Range(0, 4f)]
    public float MaxDeviationAlongY = 0.3f;

    [Range(0, 2f)]
    public float DeviationAlongX = 0.1f;

    [Range(0, 2f)]
    public float CameraShakeForce = 3f;

    [Header("- Sounds -")]
    public AudioSource FireSound;
    public AudioSource IdleSound;

    [Header("- Fire particle -")]
    public ParticleSystem FireParticleSystem;

    [Header("- Hand IK -")]
    public Transform LeftHandIk;

    // Scope settings
    [Header("- Aiming settings -")]
    public WeaponScopeSettings ScopeSettings;

    // Used after scope disactivated
    private Transform _startParent;
    private Vector3 _startLocalPosition;
    private Vector3 _startLocalRotation;

    private bool _canShoot = true;
    private bool _canReload = true;
    private bool _isReloading;
    private bool _isScoping;

    public bool CanShoot { get { return _canShoot && BulletsInMag > 0; } }
    public bool CanReload { get { return _canReload && BulletsInMag != MagCapacity && BulletsAmount > 0; } }
    public bool IsReloading { get { return _isReloading; } }

    private void OnValidate()
    {
      MagCapacity = Mathf.Max(MagCapacity, 0);
      BulletsInMag = Mathf.Clamp(BulletsInMag, 0, MagCapacity);
      BulletsAmount = Mathf.Max(BulletsAmount, 0);
      ShootFrequency = Mathf.Max(ShootFrequency, 0);

      MinDeviationAlongY = Mathf.Clamp(MinDeviationAlongY, 0, MaxDeviationAlongY);
      MaxDeviationAlongY = Mathf.Max(MaxDeviationAlongY, MinDeviationAlongY);
    }

    private void Start()
    {
      if (BulletsInMag > 0)
        _canShoot = true;
      else
        _canShoot = false;

      if (ScopeSettings.IsAimingAvailable)
      {
        UpdateParent();
      }
    }

    public void UpdateParent()
    {
      _startParent = transform.parent;
      _startLocalPosition = transform.localPosition;
      _startLocalRotation = transform.localEulerAngles;
    }

    private Vector3 _emptyFireVector = new Vector3(-90, 0, 0);

    // Shoot
    public void Fire(Vector3 positionWhereToFire)
    {
      if (_canShoot && BulletsInMag > 0)
      {
        // weapon recoil
        if (_isScoping)
        {
          transform.localEulerAngles = ScopeSettings.WeaponLocalPositionRotation.localEulerAngles + ScopeSettings.WeaponRotationShooting;
          StartCoroutine(ReturnRotationBack());
        }

        // makes shooting unavailable for shootFrequency time
        _canShoot = false;
        Invoke("CanShootNow", ShootFrequency);

        // Decrement bullet count
        BulletsInMag--;

        // Sound
        FireSound.Stop();
        FireSound.Play();

        // Particle
        FireParticleSystem.Stop();
        FireParticleSystem.Play();

        // Bullet position
        if (positionWhereToFire != Vector3.zero)
        {
          BulletPosition.LookAt(positionWhereToFire);
        }
        else
        {
          BulletPosition.eulerAngles = _emptyFireVector;
        }

        // Instantiates bullet
        Instantiate(
          BulletPrefab,
          BulletPosition.transform.position,
          BulletPosition.transform.rotation
        );
      }
      else
      {
        _canShoot = false;
        Invoke("CanShootNow", 0.4f);

        // Idle sound
        IdleSound.Play();
      }
    }

    // Makes shooting available
    private void CanShootNow()
    {
      if (_canReload)
        _canShoot = true;
    }

    // Used for shooting effect
    private float BackSpeed = 0.1f;
    private IEnumerator ReturnRotationBack()
    {
      while (_isScoping && Mathf.Abs(transform.localEulerAngles.x - ScopeSettings.WeaponLocalPositionRotation.localEulerAngles.x) > 0.005f)
      {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, ScopeSettings.WeaponLocalPositionRotation.localRotation, BackSpeed);
        yield return null;
      }

      if (_isScoping)
        transform.localEulerAngles = ScopeSettings.WeaponLocalPositionRotation.localEulerAngles;
    }

    // Get collision point
    public Vector3 GetCollisionPoint()
    {
      return PlayerBehaviour.GetInstance().FirePoint;
    }

    public void Reload()
    {
      _canReload = false;
      _canShoot = false;

      _isReloading = true;
    }

    private int _addBulletsCount;

    public void ReloadFinished()
    {
      _addBulletsCount = Mathf.Min(MagCapacity - BulletsInMag, BulletsAmount);
      BulletsInMag += _addBulletsCount;
      BulletsAmount -= _addBulletsCount;

      _canReload = true;
      _canShoot = true;

      _isReloading = false;
    }

    public void ScopeActive()
    {
      if (ScopeSettings.IsAimingAvailable && ScopeSettings.IsFPS)
      {
        transform.parent = ScopeSettings.WeaponParent;
        transform.localPosition = ScopeSettings.WeaponLocalPositionRotation.localPosition;
        transform.localRotation = ScopeSettings.WeaponLocalPositionRotation.localRotation;

        ScopeSettings.Hands?.SetActive(true);

        if (ScopeSettings.IsFPS)
          _isScoping = true;
      }
    }

    public void ScopeDisactivate()
    {
      if (ScopeSettings.IsAimingAvailable && ScopeSettings.IsFPS)
      {
        transform.parent = _startParent;
        transform.localPosition = _startLocalPosition;
        transform.localEulerAngles = _startLocalRotation;

        ScopeSettings.Hands?.SetActive(false);

        if (ScopeSettings.IsFPS)
          _isScoping = false;
      }
    }

    [System.Serializable]
    public class WeaponScopeSettings
    {
      public bool IsAimingAvailable = true;

      public float FieldOfView = 50;
      public Vector3 PlayerSpineRotation;
      public Transform CameraPosition;

      public bool IsFPS = true;
      public Vector3 WeaponRotationShooting = new Vector3(-3, 0, 0);
      public GameObject Hands;
      public Transform WeaponParent;
      public Transform WeaponLocalPositionRotation;
    }
  }
}
