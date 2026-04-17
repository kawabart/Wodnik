using System;
using UnityEngine;

[RequireComponent(typeof(AgitationController))]
[RequireComponent(typeof(EnemyPerception))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyController : MonoBehaviour
{

    #region states
    public EnemyState CurrentState = EnemyState.Alive;

    public void ChangeState(EnemyState newState)
    {
        CurrentState = newState;
        if (newState == EnemyState.Alive)
        {
            rigidBody.interpolation = RigidbodyInterpolation.None;
            behaviorAgent.enabled = true;
            navMeshAgent.enabled = true;
            rigidBody.isKinematic = true;
            agitationController.enabled = true;
            perceptionController.ActivateSenses();
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Debug.Log("Enemy recovered from being downed");
        }
        else if (newState == EnemyState.Downed)
        {
            downedTimer = DownedTime;
            rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            behaviorAgent.enabled = false;
            navMeshAgent.enabled = false;
            rigidBody.isKinematic = false;
            agitationController.enabled = true;
            perceptionController.DectivateSenses();
            Debug.Log("Enemy is downed.");
        }
        else if (newState == EnemyState.Dead)
        {
            rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            behaviorAgent.enabled = false;
            navMeshAgent.enabled = false;
            rigidBody.isKinematic = false;
            rigidBody.freezeRotation = false;
            agitationController.enabled = false;
            perceptionController.DectivateSenses();
            Debug.Log("Enemy is dead.");
        }
    }
    #endregion

    #region downed
    public float DownedTime = 10f;
    [SerializeField]
    private float downedTimer =0;
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
    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        behaviorAgent = GetComponent<Unity.Behavior.BehaviorGraphAgent>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
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

}
