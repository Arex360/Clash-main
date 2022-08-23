using UnityEngine;
using UnityEditor;

namespace TPSShooter
{
  public class VehicleCreatorEditor : EditorWindow
  {
    private GameObject cabinMesh;
    private GameObject wheelMesh;

    private AudioClip idleClip;
    private AudioClip explosionClip;

    private GameObject explosionParticle;

    private EditorStyles editorStyles;
    
    private const int axlesCount = 2;
    private const float mass = 1500;
    private const float axleStep = 2;
    private const float axleWidth = 2;
    private const float axleShift = -0.5f;

    [MenuItem("TPS Shooter/Creator Tools/Vehicle", false, 104)]
    public static void ShowWindow()
    {
      GetWindow<VehicleCreatorEditor>("Vehicle Creator").minSize = new Vector2(320, 300);
    }

    private void OnGUI()
    {
      editorStyles = (editorStyles == null) ? new EditorStyles() : editorStyles;

      EditorGUILayout.Space();
      editorStyles.ShowHeaderInfo("Vehicle Creator");
      editorStyles.ShowWrapperGUI(ShowMeshGUI);
      editorStyles.ShowWrapperGUI(ShowAudioGUI);
      editorStyles.ShowWrapperGUI(ShowParticlesGUI);

      editorStyles.AddDoubleSpace();
      if (GUILayout.Button("Create"))
      {
        CreateVehicle();
      }
    }

    private void ShowMeshGUI()
    {
      editorStyles.ShowLabelInfo("Meshes (Optional)");
      cabinMesh = EditorGUILayout.ObjectField("Cabin", cabinMesh, typeof(GameObject), true) as GameObject;
      wheelMesh = EditorGUILayout.ObjectField("Wheel", wheelMesh, typeof(GameObject), true) as GameObject;
    }

    private void ShowAudioGUI()
    {
      editorStyles.ShowLabelInfo("Sounds (Optional)");
      idleClip = EditorGUILayout.ObjectField("Idle", idleClip, typeof(AudioClip), false) as AudioClip;
      explosionClip = EditorGUILayout.ObjectField("Explosion", explosionClip, typeof(AudioClip), false) as AudioClip;
    }

    private void ShowParticlesGUI()
    {
      editorStyles.ShowLabelInfo("Particles (Optional)");
      explosionParticle = EditorGUILayout.ObjectField("Explosion Particle", explosionParticle, typeof(GameObject), false) as GameObject;
    }

    private void CreateVehicle()
    {
      GameObject root = new GameObject("Vehicle");
      root.layer = LayerMask.NameToLayer(Layers.Vehicle);

      AddCollider(root);
      AddRigidbody(root);
      AddWheelsColliders(root);

      root.AddComponent<Vehicle>();
      root.AddComponent<VehicleHealthBar>();

      AddMeshes(root);
      AddSounds(root);
      AddParticles(root);
      AddPlayerPositions(root);
    }

    private void AddCollider(GameObject root)
    {
      float length = (axlesCount - 1) * axleStep;
      BoxCollider collider = root.AddComponent<BoxCollider>();
      collider.size = new Vector3(axleWidth, 1, length);
    }

    private void AddRigidbody(GameObject root)
    {
      Rigidbody rb = root.AddComponent<Rigidbody>();
      rb.mass = mass;
    }

    private void AddWheelsColliders(GameObject root) 
    {
      float length = (axlesCount - 1) * axleStep;
      float firstOffset = length * 0.5f;

      for (int i = 0; i < axlesCount; ++i)
      {
        string leftWheelName = ((i == 0) ? "Front" : "Rear") + "LeftWheel";
        string rightWheelName = ((i == 0) ? "Front" : "Rear") + "RightWheel";
        var leftWheel = new GameObject(leftWheelName);
        var rightWheel = new GameObject(rightWheelName);

        leftWheel.AddComponent<WheelCollider>();
        rightWheel.AddComponent<WheelCollider>();

        leftWheel.transform.parent = root.transform;
        rightWheel.transform.parent = root.transform;

        leftWheel.transform.localPosition = new Vector3(-axleWidth * 0.5f, axleShift, firstOffset - axleStep * i);
        rightWheel.transform.localPosition = new Vector3(axleWidth * 0.5f, axleShift, firstOffset - axleStep * i);
      }
    }
  
    private void AddMeshes(GameObject root)
    {
      if(cabinMesh)
      {
        GameObject cabin = Instantiate(cabinMesh);
        cabin.name = "Cabin";
        cabin.transform.parent = root.transform;
        cabin.transform.localPosition = Vector3.zero;
        cabin.transform.localScale = Vector3.one;
        cabinMesh.transform.localRotation = Quaternion.identity;
      }
      if(wheelMesh)
      {
        root.GetComponent<WheelDrive>().wheelShape = wheelMesh;
      }
    }

    private void AddSounds(GameObject root)
    {
      if(idleClip)
      {
        GameObject idle = new GameObject("IdleSound");
        idle.transform.parent = root.transform;
        idle.transform.localPosition = Vector3.zero;
        idle.transform.localRotation = Quaternion.identity;

        AudioSource source = idle.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.clip = idleClip;

        root.GetComponent<Vehicle>().idleSound = source;
      }
      if(explosionClip)
      {
        GameObject idle = new GameObject("ExplosionSound");
        idle.transform.parent = root.transform;
        idle.transform.localPosition = Vector3.zero;
        idle.transform.localRotation = Quaternion.identity;

        AudioSource source = idle.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.clip = explosionClip;

        root.GetComponent<VehicleHealthBar>().explosionSound = source;
      }
    }

    private void AddParticles(GameObject root)
    {
      if(explosionParticle)
      {
        GameObject explosion = Instantiate(explosionParticle);
        explosion.name = "ExplosionParticle";
        explosion.transform.parent = root.transform;
        explosion.transform.localPosition = Vector3.zero;
        root.GetComponent<VehicleHealthBar>().explosionParticle = explosion.GetComponent<ParticleSystem>();
      }
    }

    private void AddPlayerPositions(GameObject root)
    {
      GameObject playerSit = new GameObject("PlayerSitPos");
      playerSit.transform.parent = root.transform;
      playerSit.transform.localRotation = Quaternion.identity;
      playerSit.transform.localPosition = new Vector3(0, -0.22f, -0.14f);
      root.GetComponent<Vehicle>().playerSit = playerSit.transform;

      GameObject playerStand = new GameObject("PlayerStandPos");
      playerStand.transform.parent = root.transform;
      playerStand.transform.localRotation = Quaternion.identity;
      playerStand.transform.localPosition = new Vector3(-1.75f, 0.1f, -0.5f);
      root.GetComponent<Vehicle>().playerStand = playerStand.transform;
    }
  }
}
