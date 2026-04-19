using System;
using System.Net.NetworkInformation;
using Unity.Behavior;
using UnityEngine;

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
        var behaviorAgent = GetComponent<BehaviorGraphAgent>();
        var navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

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
    #endregion

    #region dead
    public void Kill()
    {
        ChangeState(EnemyState.Dead);
    }
    #endregion

    private Rigidbody rigidBody;
    private Unity.Behavior.BehaviorGraphAgent behaviorAgent;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private CapsuleCollider capsuleCollider;
    private AgitationController agitationController;
    private EnemyPerception perceptionController;
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
