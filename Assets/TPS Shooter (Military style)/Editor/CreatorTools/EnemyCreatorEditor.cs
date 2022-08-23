using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShooter
{
  public class EnemyCreatorEditor : EditorWindow
  {
    [MenuItem("TPS Shooter/Creator Tools/Enemy", false, 103)]
    public static void ShowWindow()
    {
      GetWindow<EnemyCreatorEditor>("Enemy Creator");
    }

    private EditorStyles _editorStyles;

    private GameObject _enemyPrefab;
    private bool _isRagdoll;
    private RuntimeAnimatorController _enemyAnimator;

    private GameObject _bloodEffectParticlePrefab;
    private bool _withRadarableObject;
    private GameObject _radarableObjectPrefab;

    private GameObject _weaponPrefab;
    private GameObject _bulletPrefab;
    private AudioClip _fireClip;
    private GameObject _fireParticlePrefab;

    private void Awake()
    {
      _editorStyles = new EditorStyles();
    }

    private Vector2 _scrollPos;

    private void OnGUI()
    {
      if (_editorStyles == null)
        _editorStyles = new EditorStyles();

      _scrollPos = EditorGUILayout.BeginScrollView(
        _scrollPos,
        false,
        false, 
        GUILayout.Width(position.width),
        GUILayout.Height(position.height)
      );

      EditorGUILayout.Space();
      _editorStyles.ShowHeaderInfo("Enemy Setup");

      _editorStyles.ShowWrapperGUI(ShowMainGUI);
      _editorStyles.ShowWrapperGUI(ShowAdditionalGUI);
      _editorStyles.ShowWrapperGUI(ShowWeaponGUI);
      ShowSetupButton();

      EditorGUILayout.EndScrollView();
    }

    private void ShowMainGUI()
    {
      _editorStyles.ShowLabelInfo("Main");

      _enemyPrefab = EditorGUILayout.ObjectField("Enemy Object", _enemyPrefab, typeof(GameObject), true) as GameObject;
      _isRagdoll = EditorGUILayout.Toggle("IsRagdoll", _isRagdoll);
      _enemyAnimator = EditorGUILayout.ObjectField("Animator", _enemyAnimator, typeof(RuntimeAnimatorController), false) as RuntimeAnimatorController;

      if (_isRagdoll)
        EditorGUILayout.HelpBox("You have to setup Enemy Object as Ragdoll by yourself.", MessageType.Info);
    }

    private void ShowAdditionalGUI()
    {
      _editorStyles.ShowLabelInfo("Additional");

      _bloodEffectParticlePrefab = EditorGUILayout.ObjectField("Blood Effect", _bloodEffectParticlePrefab, typeof(GameObject), false) as GameObject;
      if (_bloodEffectParticlePrefab && _bloodEffectParticlePrefab.GetComponent<ParticleSystem>() == null)
      {
        _bloodEffectParticlePrefab = null;
        Debug.LogError("Enemy Creator: Blood Effect has to have ParticleSystem component.");
      }

      _withRadarableObject = EditorGUILayout.Toggle("Add enemy on Radar", _withRadarableObject);
      if (_withRadarableObject)
      {
        _radarableObjectPrefab = EditorGUILayout.ObjectField("Radarable Object", _radarableObjectPrefab, typeof(GameObject), false) as GameObject;
        if (_radarableObjectPrefab && _radarableObjectPrefab.GetComponent<Image>() == null)
        {
          _radarableObjectPrefab = null;
          Debug.LogError("Enemy Creator: Radarable Object has to have Image component.");
        }
      }
    }

    private void ShowWeaponGUI()
    {
      _editorStyles.ShowLabelInfo("Weapon");

      _weaponPrefab = EditorGUILayout.ObjectField("Weapon Prefab", _weaponPrefab, typeof(GameObject), false) as GameObject;
      _bulletPrefab = EditorGUILayout.ObjectField("Bullet Prefab", _bulletPrefab, typeof(GameObject), false) as GameObject;
      _fireClip = EditorGUILayout.ObjectField("Fire Audio Clip", _fireClip, typeof(AudioClip), false) as AudioClip;
      _fireParticlePrefab = EditorGUILayout.ObjectField("Fire Particle", _fireParticlePrefab, typeof(GameObject), false) as GameObject;

      if (_bulletPrefab && _bulletPrefab.GetComponent<EnemyBullet>() == null)
      {
        _bulletPrefab = null;
        Debug.LogError("Enemy Creator: Bullet Prefab has to have EnemyBullet component.");
      }
      if (_fireParticlePrefab && _fireParticlePrefab.GetComponent<ParticleSystem>() == null)
      {
        _fireParticlePrefab = null;
        Debug.LogError("Enemy Creator: Fire Particle has to have ParticleSystem component.");
      }
    }

    private void ShowSetupButton()
    {
      EditorGUILayout.Space();
      EditorGUILayout.Space();

      if (_enemyPrefab && _enemyAnimator && _bloodEffectParticlePrefab && _weaponPrefab && _fireClip && _fireParticlePrefab)
      {
        if (_withRadarableObject && _radarableObjectPrefab == null)
          return;

        if (GUILayout.Button("Setup"))
          CreateEnemy();
      }
    }

    private void CreateEnemy()
    {
      // Instantiate enemy
      GameObject enemyObject = Instantiate(_enemyPrefab);
      enemyObject.name = _enemyPrefab.name;

      // Add Enemy Behaviour
      enemyObject.AddComponent(typeof(EnemyBehaviour));
      EnemyBehaviour enemyBehaviour = enemyObject.GetComponent<EnemyBehaviour>();

      // Animator
      enemyObject.GetComponent<Animator>().runtimeAnimatorController = _enemyAnimator;
      enemyObject.GetComponent<Animator>().applyRootMotion = true;

      // Vision settings
      GameObject visionPosition = new GameObject();
      visionPosition.name = "VisionPosition";
      visionPosition.transform.SetParent(enemyObject.transform);
      visionPosition.transform.localPosition = new Vector3(0, 1.75f, 0);
      visionPosition.transform.localEulerAngles = Vector3.zero;
      enemyBehaviour.VisionSettings.VisionPosition = visionPosition.transform;

      // Death settings
      enemyBehaviour.DeathSettings.IsRagdolled = _isRagdoll;

      // Blood effect
      GameObject bloodEffect = Instantiate(_bloodEffectParticlePrefab, enemyObject.transform);
      bloodEffect.transform.localPosition = Vector3.zero;
      bloodEffect.transform.localEulerAngles = Vector3.zero;
      bloodEffect.name = "BloodEffect";
      enemyBehaviour.BloodEffect = bloodEffect.GetComponent<ParticleSystem>();

      // Footstep
      GameObject footstepAudioSource = new GameObject();
      footstepAudioSource.name = "FootstepAudioSource";
      footstepAudioSource.transform.SetParent(enemyObject.transform);
      footstepAudioSource.transform.localPosition = Vector3.zero;
      footstepAudioSource.transform.localEulerAngles = Vector3.zero;
      footstepAudioSource.AddComponent(typeof(AudioSource)).GetComponent<AudioSource>().playOnAwake = false;
      enemyObject.GetComponent<FootstepSounds>().audioSource = footstepAudioSource.GetComponent<AudioSource>();

      // Radar
      if (_withRadarableObject)
      {
        EnemyRadarableObject radarableObject = enemyObject.AddComponent(typeof(EnemyRadarableObject)) as EnemyRadarableObject;
        radarableObject.RadarableImagePrefab = _radarableObjectPrefab;
      }

      // Weapon
      GameObject weapon = Instantiate(_weaponPrefab, enemyObject.transform);
      weapon.name = _weaponPrefab.name;
      weapon.transform.localPosition = Vector3.zero;
      weapon.transform.localEulerAngles = Vector3.zero;

      EnemyWeapon enemyWeapon = weapon.AddComponent(typeof(EnemyWeapon)) as EnemyWeapon;
      enemyBehaviour.WeaponSettings.Weapon = enemyWeapon;

      // Bullet
      enemyWeapon.BulletPrefab = _bulletPrefab;

      // BulletPosition
      GameObject bulletPosition = new GameObject();
      bulletPosition.name = "BulletPosition";
      bulletPosition.transform.SetParent(enemyWeapon.transform);
      bulletPosition.transform.localPosition = Vector3.zero;
      bulletPosition.transform.localEulerAngles = Vector3.zero;

      enemyWeapon.BulletPosition = bulletPosition.transform;

      // FireSound
      GameObject fireSound = new GameObject();
      fireSound.name = "FireAudioSource";
      fireSound.transform.SetParent(weapon.transform);
      fireSound.transform.localPosition = Vector3.zero;
      fireSound.transform.localEulerAngles = Vector3.zero;

      AudioSource fireSoundAudioSource = fireSound.AddComponent(typeof(AudioSource)) as AudioSource;
      fireSoundAudioSource.playOnAwake = false;
      fireSoundAudioSource.clip = _fireClip;

      enemyWeapon.FireSound = fireSoundAudioSource;

      // Fire Particle
      GameObject fireParticle = Instantiate(_fireParticlePrefab, weapon.transform);
      fireParticle.name = "FireParticle";
      fireParticle.transform.localPosition = Vector3.zero;
      fireParticle.transform.localEulerAngles = Vector3.zero;
      enemyWeapon.FireParticleSystem = fireParticle.GetComponent<ParticleSystem>();

      Close();
    }

  }
}