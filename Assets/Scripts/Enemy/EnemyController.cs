using System;
using UnityEngine;

[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(AgitationController))]
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
            rigidBody.freezeRotation = true;
            agitationController.enabled = true;
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Debug.Log("Enemy recovered from being downed");
        }
        else if (newState == EnemyState.Downed)
        {
<<<<<<< HEAD
            rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidBody.freezeRotation = false;
            rigidBody.isKinematic = false;
            navMeshAgent.enabled = false;
=======
>>>>>>> c37fafc (refactored EnemyAI tree around enemy agitation)
            behaviorAgent.enabled = false;
            navMeshAgent.enabled = false;
            rigidBody.isKinematic = false;
            rigidBody.freezeRotation = false;
            agitationController.enabled = true;
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
            Debug.Log("Enemy is dead.");
        }
    }
    #endregion

    #region downed
    private readonly float DOWNED_TIME = 10f;
    public void BecomeDowned()
    {
        if (CurrentState == EnemyState.Dead) return;
        ChangeState(EnemyState.Downed);
        timer.Start();
    }
    #endregion

    #region dead
    public void Kill()
    {
        ChangeState(EnemyState.Dead);
        timer.Reset();
    }
    #endregion

    private Rigidbody rigidBody;
    private Unity.Behavior.BehaviorGraphAgent behaviorAgent;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private CapsuleCollider capsuleCollider;
    private Timer timer;

    private AgitationController agitationController;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        behaviorAgent = GetComponent<Unity.Behavior.BehaviorGraphAgent>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        timer = GetComponent<Timer>();
        timer.Reset();
        agitationController = GetComponent<AgitationController>();
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
        if (timer.time >= DOWNED_TIME)
        {
            timer.Reset();
            ChangeState(EnemyState.Alive);
        }
    }

    void UpdateDead()
    {
        // For future need if it will be needed.
    }

}
