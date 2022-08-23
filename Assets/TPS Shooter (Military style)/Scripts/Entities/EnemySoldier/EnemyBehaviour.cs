using UnityEngine.AI;
using UnityEngine;

using LightDev;
using LightDev.Core;

using DG.Tweening;

namespace TPSShooter
{
  [RequireComponent(typeof(FootstepSounds))]
  [RequireComponent(typeof(NavMeshAgent))]
  [RequireComponent(typeof(CharacterController))]
  [RequireComponent(typeof(Animator))]
  public partial class EnemyBehaviour : Base
  {
    public EnemyVisionSettings VisionSettings = new EnemyVisionSettings();
    public EnemyPatrollingSettings PatrollingSettings = new EnemyPatrollingSettings();
    public EnemyWeaponSettings WeaponSettings = new EnemyWeaponSettings();
    public EnemyDeathSettings DeathSettings = new EnemyDeathSettings();
    public EnemyAIBehaviour AI_Behaviour = new EnemyAIBehaviour();
    public EnemySettingsIK SettingsIK = new EnemySettingsIK();
    private EnemyAnimationParameters AnimatorParameters = new EnemyAnimationParameters();

    [Space]
    [Tooltip("Distance when Enemy starts attacking player.")]
    public float InnerAttackRadius = 35;
    [Tooltip("Distance when Enemy stop attacking player.")]
    public float OuterAttackRadius = 40f;
    public float MaxPlayerDetectionRadius = 150;

    [Space]
    public ParticleSystem BloodEffect;

    private CharacterController characterController;
    private Animator animator;
    private NavMeshAgent navmeshAgent;
    private PlayerBehaviour player;

    private EnemyBehaviourState currentState;
    private IdleState idleState;
    private PatrolState patrolState;
    private SearchState searchState;
    private ChaseState chaseState;
    private AttackState attackState;
    private DeathState deathState;
    private PlayerDiedState playerDiedState;

    private float hp = 100f;
    private bool updateSpineIK = false;
    private Vector3 spineIKLookAt;

    private const float MaxRunSpeed = 5;
    private const float MaxRunAcceleration = 20;

    private const string forwardAnimParamSequenceID = "f";
    private const string strafeAnimParamSequenceID = "s";

    public event System.Action onHpChanged;
    public event System.Action onDied;

    public float GetHP() { return hp; }
    public bool IsAlive() { return currentState != deathState; }

    private void OnValidate()
    {
      const string timeBeforeShotError = "TimeBeforeShot has to be more or equal 0 and less than EnemyWeapon.shootFrequency. It is a delay that used to stop Enemy from tracking player before shoot.";
      if (WeaponSettings.TimeBeforeShot < 0)
      {
        WeaponSettings.TimeBeforeShot = 0;
        Debug.LogError(timeBeforeShotError);
      }
      else if (WeaponSettings.Weapon && WeaponSettings.TimeBeforeShot > WeaponSettings.Weapon.ShootFrequency)
      {
        WeaponSettings.TimeBeforeShot = WeaponSettings.Weapon.ShootFrequency;
        Debug.LogError(timeBeforeShotError);
      }

      if (InnerAttackRadius < 0)
      {
        InnerAttackRadius = 0;
        Debug.LogError("InnerAttackRadius must be more than 0.");
      }

      if (OuterAttackRadius < 0)
      {
        OuterAttackRadius = 0;
        Debug.LogError("OuterAttackRadius must be more than 0.");
      }


      if (InnerAttackRadius >= OuterAttackRadius)
      {
        InnerAttackRadius = OuterAttackRadius;
        OuterAttackRadius += 0.1f;
        Debug.LogError("InnerAttackRadius must be less than OuterAttackRadius.");
      }

      if (MaxPlayerDetectionRadius <= InnerAttackRadius || MaxPlayerDetectionRadius <= OuterAttackRadius)
      {
        MaxPlayerDetectionRadius = Mathf.Max(InnerAttackRadius, OuterAttackRadius) + 0.1f;
        Debug.LogError("MaxPlayerDetectionRadius must be more than InnerAttackRadius and OuterAttackRadius.");
      }
    }

    private void Start()
    {
      idleState = new IdleState(this);
      patrolState = new PatrolState(this);
      searchState = new SearchState(this);
      chaseState = new ChaseState(this);
      attackState = (AI_Behaviour.AttackMotion == AttackMotion.None) ? new AttackState(this) : new StrafeAttackState(this);
      deathState = new DeathState(this);
      playerDiedState = new PlayerDiedState(this);

      animator = GetComponent<Animator>();
      characterController = GetComponent<CharacterController>();
      navmeshAgent = GetComponent<NavMeshAgent>();
      player = PlayerBehaviour.GetInstance();

      animator.applyRootMotion = false;
      navmeshAgent.autoBraking = false;
      InitializeStartState();

      // Sets to kinematic in order to Rigidbodies do not interact with CharacterController
      foreach (Rigidbody b in GetComponentsInChildren<Rigidbody>())
        b.isKinematic = true;

      Events.EnemyCreated.Call(this);
      Events.PlayerDied += OnPlayerDied;
    }

    private void OnDestroy()
    {
      Events.PlayerDied -= OnPlayerDied;
    }

    private void Update()
    {
      currentState.OnUpdate();
      UpdateGravity();
    }

    private void LateUpdate()
    {
      UpdateSpineIK();
    }

    private void OnPlayerDied()
    {
      if (currentState == deathState) return;

      ChangeState(playerDiedState);
    }

    private void UpdateGravity()
    {
      if (!characterController.enabled || characterController.isGrounded) return;

      Vector3 gravity = Vector3.zero;
      gravity.y = characterController.isGrounded ? -1 : Physics.gravity.y * Time.deltaTime;
      gravity.y = gravity.y - Mathf.Min(0, characterController.velocity.y);
      characterController.Move(gravity * Time.deltaTime);
    }

    private void UpdateSpineIK()
    {
      if (updateSpineIK)
      {
        Quaternion startRotation = SettingsIK.Spine.rotation;
        SettingsIK.Spine.LookAt(spineIKLookAt);
        SettingsIK.Spine.Rotate(SettingsIK.SpineRotation);
      }
    }

    public void OnVehicleCollision()
    {
      if (player.IsDrivingVehicle)
      {
        ChangeState(deathState);
      }
    }

    public void OnBulletHit(PlayerBullet bullet, float damageMultiplier)
    {
      if (currentState == deathState) return;

      hp -= bullet.damage * damageMultiplier;
      onHpChanged?.Invoke();

      if (BloodEffect != null)
      {
        BloodEffect.transform.position = bullet.transform.position;
        BloodEffect.Stop();
        BloodEffect.Play();
      }

      if (hp <= 0)
      {
        ChangeState(deathState);
      }
    }

    public void OnGrenadeHit(AbstractGrenade grenade)
    {
      ChangeState(deathState);
    }

    private void InitializeStartState()
    {
      if (HasWaypoints())
      {
        currentState = patrolState;
      }
      else
      {
        currentState = idleState;
      }
      currentState.OnEnter();
    }

    private void ChangeState(EnemyBehaviourState state)
    {
      if (currentState == state) return;

      currentState.OnExit();
      currentState = state;
      currentState.OnEnter();
    }

    private void LookAtLerp(Vector3 lookAt, float lerp = 5)
    {
      Quaternion previous = transform.rotation;
      transform.LookAt(lookAt);
      transform.rotation = Quaternion.Lerp(previous, transform.rotation, lerp * Time.deltaTime);
    }

    private void SetForwardAnimatorParameter(float value)
    {
      KillSequence(forwardAnimParamSequenceID);
      Sequence(
        DOTween.To((v) =>
        {
          animator.SetFloat(AnimatorParameters.ForwardHash, v);
        }, animator.GetFloat(AnimatorParameters.ForwardHash), value, 0.3f)
      ).stringId = forwardAnimParamSequenceID;
    }

    private void SetStrafeAnimatorParameter(float value)
    {
      KillSequence(strafeAnimParamSequenceID);
      Sequence(
        DOTween.To((v) =>
        {
          animator.SetFloat(AnimatorParameters.StrafeHash, v);
        }, animator.GetFloat(AnimatorParameters.StrafeHash), value, 0.3f)
      ).stringId = strafeAnimParamSequenceID;
    }

    private void StopNavMeshAgent()
    {
      navmeshAgent.isStopped = true;
      navmeshAgent.velocity = Vector3.zero;
    }

    private void ResumeNavMeshAgent()
    {
      navmeshAgent.isStopped = false;
    }

    private float GetDistanceToPlayer()
    {
      return Vector3.Distance(GetPosition(), player.GetPosition());
    }

    private bool HasWaypoints()
    {
      return PatrollingSettings.Waypoints.Length != 0;
    }

    private bool cachedIsPlayerRaycasted;
    private int cachedRaycastFrame = -1;
    private RaycastHit playerRaycastHit;
    private Vector3 playerPos;
    private Vector3 visionPos;
    private bool IsPlayerNoticedByRaycast()
    {
      if (cachedRaycastFrame == Time.frameCount)
      {
        return cachedIsPlayerRaycasted;
      }

      cachedRaycastFrame = Time.frameCount;
      cachedIsPlayerRaycasted = false;
      playerPos = player.GetPosition() + new Vector3(0, 1, 0);
      visionPos = VisionSettings.VisionPosition.position;
      if (Physics.Linecast(visionPos, playerPos, out playerRaycastHit, VisionSettings.VisionLayers))
      {
        if (playerRaycastHit.collider.GetComponentInParent<PlayerBehaviour>()
          || (playerRaycastHit.collider.gameObject.GetComponentInParent<Vehicle>() && player.IsDrivingVehicle))
        {
          cachedIsPlayerRaycasted = true;
        }
      }

      return cachedIsPlayerRaycasted;
    }

    private bool IsPlayerNoiseDetected()
    {
      return player.Noise > Vector3.Distance(transform.position, player.GetPosition());
    }

    private bool cachedIsPlayerInFOV;
    private int cachedFOVFrame = -1;
    private bool IsPlayerInFieldOfView()
    {
      if (cachedFOVFrame == Time.frameCount)
      {
        return cachedIsPlayerInFOV;
      }

      cachedFOVFrame = Time.frameCount;
      Vector3 targetDir = player.GetPosition() - transform.position;
      float angle = Vector3.Angle(targetDir, transform.forward);
      cachedIsPlayerInFOV = Mathf.Abs(angle) <= VisionSettings.fov;
      return cachedIsPlayerInFOV;
    }

    private bool CanChangeStateToSearch()
    {
      if (IsPlayerNoticedByRaycast()) return false;
      if (!IsPlayerNoiseDetected()) return false;
      if (GetDistanceToPlayer() > MaxPlayerDetectionRadius) return false;
      if (AI_Behaviour.SearchSettings != SearchSettings.Search) return false;

      return true;
    }

    private bool CanChangeStateToChase()
    {
      if (!IsPlayerInFieldOfView() && !IsPlayerNoiseDetected()) return false;
      if (!IsPlayerNoticedByRaycast()) return false;
      if (GetDistanceToPlayer() < OuterAttackRadius) return false;
      if (GetDistanceToPlayer() > MaxPlayerDetectionRadius) return false;
      if (AI_Behaviour.SearchSettings != SearchSettings.Search) return false;

      return true;
    }

    private bool CanChangeStateToAttack()
    {
      if (!IsPlayerInFieldOfView() && !IsPlayerNoiseDetected()) return false;
      if (!IsPlayerNoticedByRaycast()) return false;
      if (GetDistanceToPlayer() > InnerAttackRadius) return false;

      return true;
    }

    #region AdditionalClasses

    public class EnemyBehaviourDebugFriend
    {
      private EnemyBehaviour enemy;

      public EnemyBehaviourDebugFriend(EnemyBehaviour enemy)
      {
        this.enemy = enemy;
      }

      public Vector3 GetVisionPos() { return enemy.visionPos; }
      public Vector3 GetPlayerVisionPos() { return enemy.playerPos; }
      public RaycastHit GetHitPos() { return enemy.playerRaycastHit; }
      public bool IsAttacking() { return enemy.currentState == enemy.attackState; }
    }

    [System.Serializable]
    public class EnemyAnimationParameters
    {
      public string ForwardHash = "Forward";
      public string StrafeHash = "Strafe";
      public string DieHash = "Die";
    }

    [System.Serializable]
    public class EnemyVisionSettings
    {
      public float fov = 80;
      public Transform VisionPosition;
      public LayerMask VisionLayers = 1 << 0;
    }

    [System.Serializable]
    public class EnemyPatrollingSettings
    {
      public WaypointBase[] Waypoints;
    }

    [System.Serializable]
    public class WaypointBase
    {
      public Transform Destination;
      // how much time the enemy will be at this position before going on another position
      public float WaitTime;
    }

    [System.Serializable]
    public class EnemyWeaponSettings
    {
      public EnemyWeapon Weapon;
      [Tooltip("Varible has to be less than WeaponBahaviour.ShootFrequency. This variable show how much time enemy will not be looking at the player before shooting at him")]
      public float TimeBeforeShot = 0.1f;
    }

    [System.Serializable]
    public class EnemyDeathSettings
    {
      public bool IsRagdolled;
      public float EnemyDieTime = 20f;
      [Header("Items that will be unattached when enemy dies")]
      public Transform[] Items;
    }

    [System.Serializable]
    public class EnemySettingsIK
    {
      public Transform Spine;
      public Vector3 SpineRotation;
    }

    [System.Serializable]
    public class EnemyAIBehaviour
    {
      public AttackMotion AttackMotion = AttackMotion.None;
      public SearchSettings SearchSettings = SearchSettings.Search;
    }

    /// <summary>
    /// Defines whether Enemy would move in Attack state.
    /// </summary>
    [System.Serializable]
    public enum AttackMotion
    {
      None, Strafe
    }

    /// <summary>
    /// 1) Search: means that Enemy could go to states Chase and Search.
    /// After Chase/Search/Attack states Enemy would go to Idle state.
    ///
    /// 2) None: means that Enemy could not go to states Chase and Search.
    /// After Attack state Enemy would go to Patrol or Idle state depending on whether Enemy has waypoints.
    /// </summary>
    [System.Serializable]
    public enum SearchSettings
    {
      None, Search
    }

    #endregion
  }
}
