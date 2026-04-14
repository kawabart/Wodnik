using UnityEngine;

[RequireComponent(typeof(Timer))]
public class EnemyController : MonoBehaviour
{

    #region states
    public EnemyState CurrentState = EnemyState.Alive;

    public void ChangeState(EnemyState newState)
    {
        CurrentState = newState;
        if (newState == EnemyState.Alive)
        {
            navMeshAgent.enabled = true;
            rigidBody.isKinematic = true;
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Debug.Log("Enemy recovered from being downed");
        }
        else if (newState == EnemyState.Downed)
        {
            rigidBody.freezeRotation = false;
            rigidBody.isKinematic = false;
            navMeshAgent.enabled = false;
            //transform.Rotate(new Vector3(90, 0, 0));
            //rigidBody.AddForce(transform.forward * 2f, ForceMode.Impulse);
            Debug.Log("Enemy is downed.");
        }
        else if (newState == EnemyState.Dead)
        {
            //capsuleCollider.enabled = false;
            rigidBody.isKinematic = false;
            navMeshAgent.enabled = false;
            rigidBody.freezeRotation = false;
            Debug.Log("Enemy is dead.");
        }
    }
    #endregion

    #region downed
    private readonly float DOWNED_TIME = 10f;
    public void BecomeDowned()
    {
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
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private CapsuleCollider capsuleCollider;
    private Timer timer;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        timer = GetComponent<Timer>();
        timer.Reset();
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

    // This method is addded for testig purposes. It will be replaced with proper methods later.
    /*
    void OnCollisionEnter(Collision other)
    {
        if (CurrentState == EnemyState.Downed)
        {
            Kill();
        }
        else
        {
            BecomeDowned();
        }
    }
    */
}
