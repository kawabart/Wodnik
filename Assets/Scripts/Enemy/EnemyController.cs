using System;
using System.Net.NetworkInformation;
using UnityEngine;
using Unity.Behavior;

[RequireComponent(typeof(EnemyAnimationController))]
[RequireComponent(typeof(AgitationController))]
[RequireComponent(typeof(EnemyPerception))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(BehaviorGraphAgent))]
public class EnemyController : MonoBehaviour
{

    #region states
    public EnemyState CurrentState = EnemyState.Alive;
    private float aliveHeight;
    private float aliveRadius;
    private int aliveDirection;
    private Vector3 aliveCenter;

    [Header("Downed Collider Settings")]
    public float downedHeight = 0.5f;
    public float downedRadius = 0.1f;
    public int downedDirection = 2; // 0 = X axis, 1 = Y axis, 2 = Z axis
    public Vector3 downedCenter = new Vector3(0f, 0.1f, 0f);

    public void ChangeState(EnemyState newState)
    {
        const float aliveHeight = 0.9f;
        const float aliveRadius = 0.2f;
        const int aliveDirection = 1; // 1 = Y axis
        Vector3 aliveCenter = new Vector3(0f, 0.3f, 0f);

        const float downedHeight = 0.5f;
        const float downedRadius = 0.1f;
        const int downedDirection = 2; // 2 = Z axis
        Vector3 downedCenter = new Vector3(0f, downedRadius, 0f);

        CurrentState = newState;
        if (newState == EnemyState.Alive)
        {
            capsuleCollider.height = aliveHeight;
            capsuleCollider.radius = aliveRadius;
            capsuleCollider.direction = aliveDirection;
            capsuleCollider.center = aliveCenter;

            rigidBody.interpolation = RigidbodyInterpolation.None;

            behaviorAgent.enabled = true;
            navMeshAgent.enabled = true;

            rigidBody.isKinematic = true;

            agitationController.enabled = true;
            perceptionController.ActivateSenses();

            animator.SetTrigger("GetUp");
            Debug.Log("Enemy recovered from being downed");
        }
        else if (newState == EnemyState.Downed)
        {
            downedTimer = DownedTime;

            capsuleCollider.height = downedHeight;
            capsuleCollider.radius = downedRadius;
            capsuleCollider.direction = downedDirection;
            capsuleCollider.center = downedCenter;

            behaviorAgent.enabled = false;
            navMeshAgent.enabled = false;

            rigidBody.isKinematic = false;

            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;

            agitationController.enabled = true;
            perceptionController.DectivateSenses();

            animator.SetTrigger("Downed");
            Debug.Log("Enemy is downed.");
        }
        else if (newState == EnemyState.Dead)
        {
            // rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            behaviorAgent.enabled = false;
            navMeshAgent.enabled = false;

            rigidBody.isKinematic = false;
            rigidBody.freezeRotation = false;

            agitationController.enabled = false;
            perceptionController.DectivateSenses();

            animator.SetTrigger("Killed");
            Debug.Log("Enemy is dead.");
        }
    }
    #endregion

    #region downed
    [Header("Timer settings")]
    public float DownedTime = 10f;
    [SerializeField]
    private float downedTimer = 0;
    public void BecomeDowned()
    {
        if (CurrentState == EnemyState.Dead) return;
        ChangeState(EnemyState.Downed);
    }

    public void TurnPhysicsOff()
    {
        rigidBody.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }
    #endregion

    #region dead
    public void Kill()
    {
        ChangeState(EnemyState.Dead);
    }
    #endregion

    #region agitation
    /// <summary>
    /// Increases entity's agitation.
    /// </summary>
    /// <param name="input">Base increase to agitation.</param>
    /// <param name="affectedByAgitationState">Should values from current agitation config should affect this increase?</param>
    /// <param name="continous">Should this increase be affected by delta time (continous), or is it just one time input?.</param>
    /// <param name="maxAgitationFromThis">This input won't increase agitation above said number.</param>
    public void IncreaseAgitation(float input, bool affectedByAgitationState = true, bool continous = true, float maxAgitationFromThis = 100)
    {
        agitationController.IncreaseAgitation(input, affectedByAgitationState, continous, maxAgitationFromThis);
    }
    public void DecreaseAgitation()
    {
        agitationController.DecreaseAgitation();
    }
    #endregion

    #region vulnerability
    [SerializeField]
    private float blockingAngle = 80;
    public bool TryBlocking()
    {
        if (!IsVulnerable())
        {
            Debug.Log("Attack blocked!");
            agitationController.IncreaseAgitation(100,false,false);
            EffectSpawner.Instance.SpawnHit(transform.position, Vector3.up);
            GetComponent<EnemyAnimationController>().Block();
            return true;
        }
        else return false;
    }
    public bool IsVulnerable()
    {
        //enemy has no weapon
        //enemy is stunned
        //player is behind enemy
        if (CurrentState != EnemyState.Alive) return true;
        if (agitationController.AgitationState != AgitationState.Alarmed) return true;
        if (perceptionController.PerceptionState != EnemyPerceptionState.PlayerInSight) return true;
        //blocks if player is withing blocking angle from enemies facing direction
        if (Vector3.Angle(player.transform.position - transform.position, transform.forward) > blockingAngle) return true;
        return false;
    }

    #endregion

    private Rigidbody rigidBody;
    private BehaviorGraphAgent behaviorAgent;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private CapsuleCollider capsuleCollider;
    private AgitationController agitationController;
    private EnemyPerception perceptionController;
    private PlayerController player = null;
    public AgitationStateConfig CurrentAgitationConfig
    {
        get
        {
            return agitationController.CurrentAgitationConfig;
        }
    }
    private Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        behaviorAgent = GetComponent<Unity.Behavior.BehaviorGraphAgent>();

        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            aliveHeight = capsuleCollider.height;
            aliveRadius = capsuleCollider.radius;
            aliveDirection = capsuleCollider.direction;
            aliveCenter = capsuleCollider.center;
        }

        animator = GetComponentInChildren<Animator>();
        player = (PlayerController)FindAnyObjectByType(typeof(PlayerController));
        agitationController = GetComponent<AgitationController>();
        perceptionController = GetComponent<EnemyPerception>();
    }

    void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Alive:
                UpdateAlive();
                break;

            case EnemyState.Downed:
                UpdateDowned();
                break;

            case EnemyState.Dead:
                UpdateDead();
                break;
        }
    }

    void UpdateAlive()
    {

    }

    void UpdateDowned()
    {
        if (downedTimer <= 0)
            ChangeState(EnemyState.Alive);
        else
            downedTimer -= Time.deltaTime;
    }

    void UpdateDead()
    {
        // For future need if it will be needed.
    }

    // This method is addded for testig purposes. It will be replaced with proper methods later.
    // void OnCollisionEnter(Collision other)
    // {
    //     if (CurrentState == EnemyState.Downed)
    //     {
    //         Kill();
    //     }
    //     else
    //     {
    //         BecomeDowned();
    //     }
    // }
}
