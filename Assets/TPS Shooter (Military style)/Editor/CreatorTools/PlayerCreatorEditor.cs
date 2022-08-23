using UnityEngine;
using UnityEditor;

namespace TPSShooter
{
  public class PlayerCreatorEditor : EditorWindow
  {
    // Main
    private GameObject _playerObject;
    private RuntimeAnimatorController _playerAnimator;

    // Sounds
    private bool _withSounds;
    private AudioClip _jumpClip;
    private AudioClip _landClip;
    private AudioClip _changeWeaponClip;
    private AudioClip _zoomInClip;
    private AudioClip _zoomOutClip;

    // Grenade
    private bool _withGrenade;
    private GameObject _grenagePrefab;

    // Vehicle ability
    private bool _withVehicleAbility;

    // Second stage setup
    private Transform _spineTransform;
    private Transform _grenadeParent;
    private GameObject _skin;

    private Transform _player;

    private bool _firstStageButtonPressed;
    private bool _secondStageButtonPressed;

    private bool _firstStageSetupped;
    private bool _secondStageSetupped;

    private EditorStyles _editorStyles;

    [MenuItem("TPS Shooter/Creator Tools/Player", false, 101)]
    public static void ShowWindow()
    {
      GetWindow<PlayerCreatorEditor>("Player Creator");
    }

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

      if (!_firstStageSetupped)
        ShowFirstSetupStage();
      else if (!_secondStageSetupped)
        ShowSecondSetupStage();
      else
        Close();

      EditorGUILayout.EndScrollView();
    }

    private void ShowFirstSetupStage()
    {
      EditorGUILayout.Space();
      _editorStyles.ShowHeaderInfo("1/2 Setup stage");

      _editorStyles.ShowWrapperGUI(ShowMainGUI);
      _editorStyles.ShowWrapperGUI(ShowSoundsGUI);
      _editorStyles.ShowWrapperGUI(ShowGrenadeGUI);
      _editorStyles.ShowWrapperGUI(ShowVehicleAbilityGUI);

      ShowFirstStageErrors();

      _editorStyles.AddDoubleSpace();
      ShowFirstStageSetupButton();
    }

    private void ShowMainGUI()
    {
      _editorStyles.ShowLabelInfo("Main (Required)");

      _playerObject = EditorGUILayout.ObjectField("Player Object", _playerObject, typeof(GameObject), false) as GameObject;
      _playerAnimator = EditorGUILayout.ObjectField("Animator", _playerAnimator, typeof(RuntimeAnimatorController), false) as RuntimeAnimatorController;
    }

    private void ShowSoundsGUI()
    {
      _editorStyles.ShowLabelInfo("Sounds (Optional)");

      _withSounds = EditorGUILayout.Toggle("With Sounds", _withSounds);
      if (_withSounds)
      {
        _jumpClip = EditorGUILayout.ObjectField("Jump Clip", _jumpClip, typeof(AudioClip), false) as AudioClip;
        _landClip = EditorGUILayout.ObjectField("Land Clip", _landClip, typeof(AudioClip), false) as AudioClip;
        _changeWeaponClip = EditorGUILayout.ObjectField("Change Weapon Clip", _changeWeaponClip, typeof(AudioClip), false) as AudioClip;
        _zoomInClip = EditorGUILayout.ObjectField("ZoomIn Clip", _zoomInClip, typeof(AudioClip), false) as AudioClip;
        _zoomOutClip = EditorGUILayout.ObjectField("ZoomOut Clip", _zoomOutClip, typeof(AudioClip), false) as AudioClip;
      }
    }

    private void ShowGrenadeGUI()
    {
      _editorStyles.ShowLabelInfo("Grenade (Optional)");

      _withGrenade = EditorGUILayout.Toggle("With Grenade", _withGrenade);
      if (_withGrenade)
      {
        _grenagePrefab = EditorGUILayout.ObjectField("Grenade Prefab", _grenagePrefab, typeof(GameObject), false) as GameObject;
      }
    }

    private void ShowVehicleAbilityGUI()
    {
      _editorStyles.ShowLabelInfo("Vehicle Ability (Optional)");
      _withVehicleAbility = EditorGUILayout.Toggle("With Vehicle Ability", _withVehicleAbility);
    }

    private void ShowFirstStageErrors()
    {
      if (_firstStageButtonPressed)
      {
        if (_playerObject == null)
          _editorStyles.ShowErrorHelpBox("Assign Player Object");
        if (_playerAnimator == null)
          _editorStyles.ShowErrorHelpBox("Assign Animator");
      }
    }

    private void ShowFirstStageSetupButton()
    {
      if (GUILayout.Button("Add to scene"))
      {
        _firstStageButtonPressed = true;
        if (_playerObject == null || _playerAnimator == null)
          return;

        Transform cameraRig = InstantiateCameraRig();
        Transform player = InstantiatePlayer();

        cameraRig.GetComponent<TPSCamera>().target = player;

        // Vehicle ability
        if (_withVehicleAbility)
          player.GetComponent<PlayerVehicleAbility>().tpsCamera = cameraRig.GetComponent<TPSCamera>();

        // Setup IK settings
        GameObject playerLookAt = new GameObject();
        playerLookAt.name = "PlayerLookAt";
        playerLookAt.transform.parent = cameraRig.GetComponent<TPSCamera>().pivot;
        playerLookAt.transform.localPosition = new Vector3(0, 0, 50);
        player.GetComponent<PlayerBehaviour>().IkSettings.LookAt = playerLookAt.transform;

        _firstStageSetupped = true;
        _player = player.transform;
      }
    }

    private void ShowSecondSetupStage()
    {
      EditorGUILayout.Space();
      _editorStyles.ShowHeaderInfo("2/2 Setup stage");

      _editorStyles.ShowWrapperGUI(ShowIkGUI);
      if (_withGrenade)
        _editorStyles.ShowWrapperGUI(ShowSecondStageGrenadeGUI);
      if (_withVehicleAbility)
        _editorStyles.ShowWrapperGUI(ShowVehicleSecondStageGUI);

      ShowSecondStageErrors();

      _editorStyles.AddDoubleSpace();

      ShowSecondStageSetupButton();
    }

    private void ShowIkGUI()
    {
      _editorStyles.ShowLabelInfo("IK Settings");

      _spineTransform = EditorGUILayout.ObjectField("Spine", _spineTransform, typeof(Transform), true) as Transform;
    }

    private void ShowSecondStageGrenadeGUI()
    {
      _editorStyles.ShowLabelInfo("Grenade Settings");
      _grenadeParent = EditorGUILayout.ObjectField("Grenade Left Arm Transform", _grenadeParent, typeof(Transform), true) as Transform;
    }

    private void ShowVehicleSecondStageGUI()
    {
      _editorStyles.ShowLabelInfo("Vehicle Settings");
      _skin = EditorGUILayout.ObjectField("Skin", _skin, typeof(GameObject), true) as GameObject;
    }

    private void ShowSecondStageErrors()
    {
      if (_secondStageButtonPressed)
      {
        if (_spineTransform == null)
          _editorStyles.ShowErrorHelpBox("Setup Spine");
        if (_withGrenade && _grenadeParent == null)
          _editorStyles.ShowErrorHelpBox("Setup Grenade Parent Transform");
        if (_withVehicleAbility && _skin == null)
          _editorStyles.ShowErrorHelpBox("Setup Skin");
      }
    }

    private void ShowSecondStageSetupButton()
    {
      if (GUILayout.Button("Setup"))
      {
        _secondStageButtonPressed = true;

        if (_spineTransform == null)
          return;
        if (_withGrenade && _grenadeParent == null)
          return;
        if (_withVehicleAbility && _skin == null)
          return;

        _player.GetComponent<PlayerBehaviour>().IkSettings.Spine = _spineTransform;
        if (_withGrenade)
        {
          GameObject grenadePosition = new GameObject();
          grenadePosition.name = "GrenadePosition";
          grenadePosition.transform.parent = _grenadeParent;
          grenadePosition.transform.localPosition = Vector3.zero;

          _player.GetComponent<PlayerBehaviour>().grenadeSettings.GrenadeArmParent = _grenadeParent;
          _player.GetComponent<PlayerBehaviour>().grenadeSettings.GrenadePosition = grenadePosition.transform;
        }

        if (_withVehicleAbility)
        {
          _player.GetComponent<PlayerVehicleAbility>().skin = _skin;
        }

        _secondStageSetupped = true;
      }
    }

    private Transform InstantiateCameraRig()
    {
      GameObject rig = new GameObject();
      rig.name = "CameraRig";
      TPSCamera tpsCamera = rig.AddComponent(typeof(TPSCamera)) as TPSCamera;

      GameObject pivot = new GameObject();
      pivot.name = "Pivot";
      pivot.transform.parent = rig.transform;
      pivot.transform.localPosition = new Vector3(0, 1.3f, 0);
      tpsCamera.pivot = pivot.transform;

      GameObject cameraContainer = new GameObject();
      cameraContainer.name = "CameraContainer";
      cameraContainer.transform.parent = pivot.transform;
      cameraContainer.transform.localPosition = new Vector3(0.2f, 0.5f, -2.25f);
      tpsCamera.cameraContainer = cameraContainer.transform;

      GameObject camera = new GameObject();
      camera.name = "MainCamera";
      camera.tag = "MainCamera";
      camera.transform.parent = cameraContainer.transform;
      camera.transform.localPosition = Vector3.zero;
      camera.AddComponent(typeof(Camera));
      camera.GetComponent<Camera>().nearClipPlane = 0.01f;
      camera.AddComponent(typeof(AudioListener));
      tpsCamera.cameraTransform = camera.transform;

      return rig.transform;
    }

    private Transform InstantiatePlayer()
    {
      // Main
      GameObject player = Instantiate(_playerObject);
      player.name = "Player";
      player.layer = LayerMask.NameToLayer(Layers.Player);

      PlayerBehaviour playerBehaviour = player.AddComponent(typeof(PlayerBehaviour)) as PlayerBehaviour;
      player.GetComponent<Animator>().runtimeAnimatorController = _playerAnimator;

      // Sounds
      GameObject soundGameObject = new GameObject();
      AudioSource playerAudioSource = soundGameObject.AddComponent(typeof(AudioSource)) as AudioSource;
      playerAudioSource.playOnAwake = false;
      soundGameObject.name = "PlayerAudioSource";
      soundGameObject.transform.parent = player.transform;
      soundGameObject.transform.localPosition = Vector3.zero;

      playerBehaviour.sounds.AudioSource = playerAudioSource;
      if (_withSounds)
      {
        playerBehaviour.sounds.JumpSound = _jumpClip;
        playerBehaviour.sounds.LandSound = _landClip;
        playerBehaviour.sounds.ChangeWeaponSound = _changeWeaponClip;
        playerBehaviour.sounds.ZoomInSound = _zoomInClip;
        playerBehaviour.sounds.ZoomOutSound = _zoomOutClip;
      }

      // Setup grenade
      if (_withGrenade)
        playerBehaviour.grenadeSettings.GrenadePrefab = _grenagePrefab;

      // Vehicle
      if (_withVehicleAbility)
      {
        player.AddComponent(typeof(PlayerVehicleAbility));
      }

      // Footstep sounds
      GameObject footstepGameObject = new GameObject();
      AudioSource footstepAudioSource = footstepGameObject.AddComponent(typeof(AudioSource)) as AudioSource;
      footstepAudioSource.playOnAwake = false;
      footstepGameObject.name = "FootstepAudioSource";
      footstepGameObject.transform.parent = player.transform;
      footstepGameObject.transform.localPosition = Vector3.zero;

      player.GetComponent<FootstepSounds>().audioSource = footstepAudioSource;

      return player.transform;
    }
  }
}
