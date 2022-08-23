using UnityEditor;
using UnityEngine;

namespace TPSShooter
{
  public class WeaponCreatorEditor : EditorWindow
  {
    [MenuItem("TPS Shooter/Creator Tools/Player Weapon", false, 102)]
    public static void ShowWindow()
    {
      GetWindow<WeaponCreatorEditor>("Weapon Creator");
    }

    private PlayerBehaviour _playerBehaviour;
    private Transform _playerRightHand;
    private bool _setupAsCurrentWeapon = true;

    private GameObject _weaponPrefab;
    private GameObject _bulletPrefab;

    private int _magCapacity = 30;
    private float _shootFrequency = 0.09f;

    private float _minDeviationAlongY;
    private float _maxDeviationAlongY;
    private float _maxDeviationAlongX;

    private float _cameraShakeForce;

    private AudioClip _fireClip;
    private AudioClip _idleClip;

    private GameObject _fireParticlePrefab;

    private bool _withScopeAbility;
    private int _fieldOfView = 45;

    private bool _isFirstPersonView;
    private GameObject _handsPrefab;

    private EditorStyles _editorStyles;

    private void Awake()
    {
      _editorStyles = new EditorStyles();
    }

    private Vector2 _scrollPos;

    private void OnGUI()
    {
      if (_editorStyles == null)
        _editorStyles = new EditorStyles();

      _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false, GUILayout.Width(position.width),
                 GUILayout.Height(position.height));

      EditorGUILayout.Space();
      _editorStyles.ShowHeaderInfo("Weapon Setup");

      _editorStyles.ShowWrapperGUI(ShowMainGUI);
      _editorStyles.ShowWrapperGUI(ShowRecoilGUI);
      _editorStyles.ShowWrapperGUI(ShowSoundsGUI);
      _editorStyles.ShowWrapperGUI(ShowVisualEffectsGUI);
      _editorStyles.ShowWrapperGUI(ShowScopeGUI);
      _editorStyles.ShowWrapperGUI(ShowSetupGUI);

      EditorGUILayout.EndScrollView();
    }

    private void ShowMainGUI()
    {
      _editorStyles.ShowLabelInfo("Main");
      _weaponPrefab = EditorGUILayout.ObjectField("Weapon Prefab", _weaponPrefab, typeof(GameObject), false) as GameObject;
      _bulletPrefab = EditorGUILayout.ObjectField("Bullet Prefab", _bulletPrefab, typeof(GameObject), false) as GameObject;
      if (_bulletPrefab && _bulletPrefab.GetComponent<PlayerBullet>() == null)
      {
        _bulletPrefab = null;
        Debug.LogError("WeaponCreator: Bullet has to have PlayerBullet component.");
      }

      _magCapacity = EditorGUILayout.IntField("Magazine Capacity", _magCapacity);
      _shootFrequency = EditorGUILayout.FloatField("Shoot Frequency", _shootFrequency);

      if (_magCapacity < 0)
        _magCapacity = 0;
      if (_shootFrequency < 0)
        _shootFrequency = 0;
    }

    private float _maxDeviationValueY = 4;
    private void ShowRecoilGUI()
    {
      _editorStyles.ShowLabelInfo("Recoil");

      _minDeviationAlongY = EditorGUILayout.FloatField("Min Deviation Y", _minDeviationAlongY);
      _maxDeviationAlongY = EditorGUILayout.FloatField("Max Deviation Y", _maxDeviationAlongY);
      EditorGUILayout.MinMaxSlider("Deviation Y", ref _minDeviationAlongY, ref _maxDeviationAlongY, 0, _maxDeviationValueY);

      if (_maxDeviationAlongY < _minDeviationAlongY)
        _maxDeviationAlongY = _minDeviationAlongY;
      _maxDeviationAlongY = Mathf.Clamp(_maxDeviationAlongY, 0, _maxDeviationValueY);
      _minDeviationAlongY = Mathf.Clamp(_minDeviationAlongY, 0, _maxDeviationValueY);

      _maxDeviationAlongX = EditorGUILayout.Slider("Deviation X", _maxDeviationAlongX, 0, 2);

      _cameraShakeForce = EditorGUILayout.Slider("Camera Shake", _cameraShakeForce, 0, 2);
    }

    private void ShowSoundsGUI()
    {
      _editorStyles.ShowLabelInfo("Sounds");

      _fireClip = EditorGUILayout.ObjectField("Fire Clip", _fireClip, typeof(AudioClip), false) as AudioClip;
      _idleClip = EditorGUILayout.ObjectField("Idle Fire Clip", _idleClip, typeof(AudioClip), false) as AudioClip;
    }

    private void ShowVisualEffectsGUI()
    {
      _editorStyles.ShowLabelInfo("Visual Effects");

      _fireParticlePrefab = EditorGUILayout.ObjectField("Fire Particle Prefab", _fireParticlePrefab, typeof(GameObject), false) as GameObject;
      if (_fireParticlePrefab && _fireParticlePrefab.GetComponent<ParticleSystem>() == null)
      {
        _fireParticlePrefab = null;
        Debug.LogError("WeaponCreator: Fire Particle Prefab has to have ParticleSystem component.");
      }
    }

    private void ShowScopeGUI()
    {
      _editorStyles.ShowLabelInfo("Scope Ability");

      _withScopeAbility = EditorGUILayout.Toggle("Scope Ability", _withScopeAbility);
      if (_withScopeAbility)
      {
        _fieldOfView = EditorGUILayout.IntField("Field Of View", _fieldOfView);

        _isFirstPersonView = EditorGUILayout.Toggle("First Person View", _isFirstPersonView);
        if (_isFirstPersonView)
        {
          _handsPrefab = EditorGUILayout.ObjectField("Hands Prefab", _handsPrefab, typeof(GameObject), false) as GameObject;
        }
      }
    }

    private bool _buttonWasPressed;
    private bool _withoutProblems;
    private void ShowSetupGUI()
    {
      _editorStyles.ShowLabelInfo("Setup");

      _playerBehaviour = EditorGUILayout.ObjectField("Player", _playerBehaviour, typeof(PlayerBehaviour), true) as PlayerBehaviour;

      _playerRightHand = EditorGUILayout.ObjectField("Player Right Hand", _playerRightHand, typeof(Transform), true) as Transform;
      _setupAsCurrentWeapon = EditorGUILayout.Toggle("Setup As Current Weapon", _setupAsCurrentWeapon);

      if (_buttonWasPressed)
      {
        ShowProblems();
      }

      _editorStyles.AddDoubleSpace();
      if (GUILayout.Button("Setup"))
      {
        _buttonWasPressed = true;
        if (_withoutProblems)
          InstantiateWeapon();
      }
    }

    private void ShowProblems()
    {
      _withoutProblems = true;
      if (_weaponPrefab == null)
      {
        _editorStyles.ShowErrorHelpBox("Setup Weapon Prefab");
        _withoutProblems = false;
      }
      if (_bulletPrefab == null)
      {
        _editorStyles.ShowErrorHelpBox("Setup Bullet Prefab");
        _withoutProblems = false;
      }
      if (_fireClip == null || _idleClip == null)
      {
        _editorStyles.ShowErrorHelpBox("Setup Audio Clip");
        _withoutProblems = false;
      }
      if (_fireParticlePrefab == null)
      {
        _editorStyles.ShowErrorHelpBox("Setup Fire Particle");
        _withoutProblems = false;
      }
      if (_playerBehaviour == null)
      {
        _editorStyles.ShowErrorHelpBox("Setup Player");
        _withoutProblems = false;
      }
      if (_playerRightHand == null)
      {
        _editorStyles.ShowErrorHelpBox("Setup Player Hand");
        _withoutProblems = false;
      }

      if (_withScopeAbility && _isFirstPersonView && _handsPrefab == null)
      {
        _editorStyles.ShowErrorHelpBox("Setup Hands Prefab");
        _withoutProblems = false;
      }
    }

    private void InstantiateWeapon()
    {
      GameObject weapon = Instantiate(_weaponPrefab);
      weapon.name = _weaponPrefab.name;
      weapon.transform.SetParent(_playerRightHand);
      weapon.transform.localPosition = Vector3.zero;

      PlayerWeapon playerWeapon = weapon.AddComponent(typeof(PlayerWeapon)) as PlayerWeapon;

      // Bullet setup
      playerWeapon.BulletPrefab = _bulletPrefab;

      GameObject bulletPosition = new GameObject();
      bulletPosition.name = "BulletPosition";
      bulletPosition.transform.SetParent(weapon.transform);
      bulletPosition.transform.localPosition = Vector3.zero;
      playerWeapon.BulletPosition = bulletPosition.transform;

      // Weapon settings
      playerWeapon.MagCapacity = _magCapacity;
      playerWeapon.BulletsInMag = _magCapacity;
      playerWeapon.BulletsAmount = _magCapacity * 2;
      playerWeapon.ShootFrequency = _shootFrequency;

      // Recoil
      playerWeapon.MinDeviationAlongY = _minDeviationAlongY;
      playerWeapon.MaxDeviationAlongY = _maxDeviationAlongY;
      playerWeapon.DeviationAlongX = _maxDeviationAlongX;
      playerWeapon.CameraShakeForce = _cameraShakeForce;

      // Sounds
      GameObject fireSoundObject = new GameObject();
      fireSoundObject.name = "FireSound";
      fireSoundObject.transform.SetParent(weapon.transform);
      fireSoundObject.transform.localPosition = Vector3.zero;
      AudioSource fireAudioSource = fireSoundObject.AddComponent(typeof(AudioSource)) as AudioSource;
      fireAudioSource.playOnAwake = false;
      fireAudioSource.clip = _fireClip;
      playerWeapon.FireSound = fireAudioSource;

      GameObject idleFireSoundObject = new GameObject();
      idleFireSoundObject.name = "IdleFireSound";
      idleFireSoundObject.transform.SetParent(weapon.transform);
      idleFireSoundObject.transform.localPosition = Vector3.zero;
      AudioSource idleFireAudioSource = idleFireSoundObject.AddComponent(typeof(AudioSource)) as AudioSource;
      idleFireAudioSource.playOnAwake = false;
      idleFireAudioSource.clip = _idleClip;
      playerWeapon.IdleSound = idleFireAudioSource;

      // FireParticle
      GameObject fireParticleObject = Instantiate(_fireParticlePrefab);
      fireParticleObject.name = "FireParticle";
      fireParticleObject.transform.SetParent(weapon.transform);
      fireParticleObject.transform.localPosition = Vector3.zero;
      playerWeapon.FireParticleSystem = fireParticleObject.GetComponent<ParticleSystem>();

      // Hand IK
      GameObject leftHandIK = new GameObject();
      leftHandIK.name = "LeftHandIK";
      leftHandIK.transform.SetParent(weapon.transform);
      leftHandIK.transform.localPosition = Vector3.zero;
      playerWeapon.LeftHandIk = leftHandIK.transform;

      // Scope settings
      if (_withScopeAbility)
      {
        playerWeapon.ScopeSettings = new PlayerWeapon.WeaponScopeSettings();
        PlayerWeapon.WeaponScopeSettings scopeSettings = playerWeapon.ScopeSettings;
        scopeSettings.IsAimingAvailable = true;
        scopeSettings.FieldOfView = _fieldOfView;

        TPSCamera cameraBehaviour = Camera.main.transform.root.GetComponent<TPSCamera>();

        if (_isFirstPersonView)
        {
          scopeSettings.IsFPS = true;

          GameObject hands = Instantiate(_handsPrefab);
          hands.name = "Hands";
          hands.transform.SetParent(weapon.transform);
          hands.transform.localPosition = Vector3.zero;
          hands.transform.localEulerAngles = Vector3.zero;
          hands.SetActive(false);

          scopeSettings.Hands = hands;

          GameObject weaponHolder;
          if (cameraBehaviour.GetComponentInChildren<WeaponSway>() == null)
          {
            weaponHolder = new GameObject();
            weaponHolder.name = "ScopeWeaponHolder";
            weaponHolder.transform.SetParent(cameraBehaviour.pivot);
            weaponHolder.transform.localPosition = new Vector3(0.27f, 0.03f, 0.5f);
            weaponHolder.transform.localEulerAngles = Vector3.zero;
            weaponHolder.AddComponent(typeof(WeaponSway));
            weaponHolder.AddComponent(typeof(HeadBob));
          }
          else
          {
            weaponHolder = cameraBehaviour.GetComponentInChildren<WeaponSway>().gameObject;
          }

          GameObject weaponLocalPositionRotation = new GameObject();
          weaponLocalPositionRotation.name = weapon.name + "WeaponScopeLocalPosition";
          weaponLocalPositionRotation.transform.SetParent(weaponHolder.transform);
          weaponLocalPositionRotation.transform.localPosition = Vector3.zero;
          weaponLocalPositionRotation.transform.localEulerAngles = Vector3.zero;

          GameObject cameraPosition = new GameObject();
          cameraPosition.name = weapon.name + "CameraScopePosition";
          cameraPosition.transform.SetParent(cameraBehaviour.pivot);
          cameraPosition.transform.position = weaponLocalPositionRotation.transform.position;
          cameraPosition.transform.rotation = weaponLocalPositionRotation.transform.rotation;

          scopeSettings.CameraPosition = cameraPosition.transform;
          scopeSettings.WeaponParent = weaponHolder.transform;
          scopeSettings.WeaponLocalPositionRotation = weaponLocalPositionRotation.transform;
        }
        else
        {
          scopeSettings.IsFPS = false;

          GameObject cameraPosition = new GameObject();
          cameraPosition.name = weapon.name + "CameraScopePosition";

          cameraPosition.transform.SetParent(cameraBehaviour.pivot);
          cameraPosition.transform.localPosition = new Vector3(0.55f, 0.14f, -1.3f);

          scopeSettings.CameraPosition = cameraPosition.transform;
        }
      }

      // Update player's weapons
      if (_playerBehaviour.weaponSettings.AllWeapons != null)
      {
        PlayerWeapon[] weapons = new PlayerWeapon[_playerBehaviour.weaponSettings.AllWeapons.Length + 1];
        for (int i = 0; i < weapons.Length - 1; i++)
          weapons[i] = _playerBehaviour.weaponSettings.AllWeapons[i];
        weapons[weapons.Length - 1] = playerWeapon;
        _playerBehaviour.weaponSettings.AllWeapons = weapons;
      }
      else
      {
        _playerBehaviour.weaponSettings.AllWeapons = new PlayerWeapon[] { playerWeapon };
      }

      if (_setupAsCurrentWeapon)
        _playerBehaviour.weaponSettings.CurrentWeapon = playerWeapon;

      Close();
    }

  }

}