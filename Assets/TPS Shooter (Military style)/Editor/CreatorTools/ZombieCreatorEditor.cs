using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShooter
{
  public class ZombieCreatorEditor : EditorWindow
  {
    [MenuItem("TPS Shooter/Creator Tools/Zombie", false, 103)]
    public static void ShowWindow()
    {
      var window = GetWindow<ZombieCreatorEditor>("Zombie Creator");
      window.minSize = new Vector2(400, 270);
    }

    private EditorStyles _editorStyles;

    private GameObject _meshPrefab;
    private RuntimeAnimatorController _animator;
    private GameObject _bloodParticle;

    private bool _withRadarableObject;
    private GameObject _radarableObjectPrefab;

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
      _editorStyles.ShowHeaderInfo("Zombie Setup");

      _editorStyles.ShowWrapperGUI(ShowMainGUI);
      _editorStyles.ShowWrapperGUI(ShowAdditionalGUI);
      ShowSetupButton();

      EditorGUILayout.EndScrollView();
    }

    private void ShowMainGUI()
    {
      _editorStyles.ShowLabelInfo("Main");

      _meshPrefab = EditorGUILayout.ObjectField("Zombie Object", _meshPrefab, typeof(GameObject), true) as GameObject;
      _animator = EditorGUILayout.ObjectField("Animator", _animator, typeof(RuntimeAnimatorController), false) as RuntimeAnimatorController;
    }

    private void ShowAdditionalGUI()
    {
      _editorStyles.ShowLabelInfo("Additional");

      _bloodParticle = EditorGUILayout.ObjectField("Blood Effect", _bloodParticle, typeof(GameObject), false) as GameObject;
      if (_bloodParticle && _bloodParticle.GetComponent<ParticleSystem>() == null)
      {
        _bloodParticle = null;
        Debug.LogError("Zombie Creator: Blood Effect has to have ParticleSystem component.");
      }

      _withRadarableObject = EditorGUILayout.Toggle("Add Zombie on Radar", _withRadarableObject);
      if (_withRadarableObject)
      {
        _radarableObjectPrefab = EditorGUILayout.ObjectField("Radarable Object", _radarableObjectPrefab, typeof(GameObject), false) as GameObject;
        if (_radarableObjectPrefab && _radarableObjectPrefab.GetComponent<Image>() == null)
        {
          _radarableObjectPrefab = null;
          Debug.LogError("Zombie Creator: Radarable Object has to have Image component.");
        }
      }
    }

    private void ShowSetupButton()
    {
      EditorGUILayout.Space();
      EditorGUILayout.Space();

      if (_meshPrefab && _animator)
      {
        if (_withRadarableObject && _radarableObjectPrefab == null)
          return;

        if (GUILayout.Button("Setup"))
          CreateZombie();
      }
    }

    private void CreateZombie()
    {
      // Instantiate Zombie
      GameObject zombieObject = Instantiate(_meshPrefab);
      zombieObject.name = _meshPrefab.name;

      // Add Enemy Behaviour
      zombieObject.AddComponent(typeof(ZombieBehaviour));
      ZombieBehaviour zombieBehaviour = zombieObject.GetComponent<ZombieBehaviour>();

      // Animator
      zombieObject.GetComponent<Animator>().runtimeAnimatorController = _animator;
      zombieObject.GetComponent<Animator>().applyRootMotion = true;

      // Vision settings
      GameObject visionPosition = new GameObject();
      visionPosition.name = "VisionPosition";
      visionPosition.transform.SetParent(zombieObject.transform);
      visionPosition.transform.localPosition = new Vector3(0, 1.7f, 0);
      visionPosition.transform.localEulerAngles = Vector3.zero;
      zombieBehaviour.visionPosition = visionPosition.transform;

      // Blood effect
      if (_bloodParticle != null)
      {
        GameObject bloodEffect = Instantiate(_bloodParticle, zombieObject.transform);
        bloodEffect.transform.localPosition = Vector3.zero;
        bloodEffect.transform.localEulerAngles = Vector3.zero;
        bloodEffect.name = "BloodEffect";
        zombieBehaviour.bloodEffect = bloodEffect.GetComponent<ParticleSystem>();
      }

      // Radar
      if (_withRadarableObject)
      {
        ZombieRadarableObject radarableObject = zombieObject.AddComponent(typeof(ZombieRadarableObject)) as ZombieRadarableObject;
        radarableObject.RadarableImagePrefab = _radarableObjectPrefab;
      }

      Close();
    }
  }
}
