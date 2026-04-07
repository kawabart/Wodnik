using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region states
    public enum EnemyState
    {
        Alive, Downed, Dead
    }
    public EnemyState CurrentState = EnemyState.Alive;

    public void ChangeStates(EnemyState newState)
    {
        CurrentState = newState;

        if (newState == EnemyState.Downed)
        {
            rigidBody.freezeRotation = false;
            Debug.Log("Enemy is downed.");
        }
        else if (newState == EnemyState.Dead)
        {
            capsuleCollider.enabled = false;
            rigidBody.isKinematic = true;
        }
    }
    #endregion

    #region taking damage
    public void TakeDamage()
    {
        ChangeStates(EnemyState.Downed);
    }

    #endregion

    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
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

    void OnCollisionEnter(Collision collision)
    {
        TakeDamage();
    }

    void UpdateAlive()
    {

    }

    void UpdateDowned()
    {

    }

    void UpdateDead()
    {

    }
}
