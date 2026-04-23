using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(BehaviorGraphAgent))]
public class EnemyAnimationController : MonoBehaviour
{

    public void ChangeCombatState()
    {
            EnemyPerceptionState currentPerceptionState = enemyPerception.PerceptionState;
            if (currentPerceptionState == EnemyPerceptionState.PlayerInSight)
            {
                animator.SetBool(isInCombatHash, true);
                animator.SetBool(isInInvestigative, false);
            }
            else
            {
                animator.SetBool(isInCombatHash, false);
                animator.SetBool(isInInvestigative, true);
            }
    }
    public void Block()
    {
        animator.SetTrigger(blockHash);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger(attackHash);
    }
    private EnemyController enemyController;
    private EnemyPerception enemyPerception;
    private AgitationController agitationController;
    private BehaviorGraphAgent behaviorAgent;
    private Animator animator;
    private MeleeDamageDealer meleeDamageDealer;
    private int isInCombatHash;
    private int isInInvestigative;
    private int attackHash;
    private int speedHash;
    private int vForwardHash;
    private int vRightHash;
    private int blockHash;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        enemyController = GetComponent<EnemyController>();
        enemyPerception = GetComponent<EnemyPerception>();
        agitationController = GetComponent<AgitationController>();
        animator = GetComponentInChildren<Animator>();
        meleeDamageDealer = GetComponent<MeleeDamageDealer>();
        isInCombatHash = Animator.StringToHash("IsInCombat");
        isInInvestigative = Animator.StringToHash("IsInInvestigative");
        attackHash = Animator.StringToHash("Attack");
        speedHash = Animator.StringToHash("Speed");
        vForwardHash = Animator.StringToHash("vForward");
        vRightHash = Animator.StringToHash("vRight");
        blockHash = Animator.StringToHash("Block");
    }

    void Update()
    {
        
        if (navMeshAgent.enabled)
        {
            float speed = navMeshAgent.velocity.magnitude;
            animator.SetFloat(speedHash, speed);
        }
        else
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;

            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            animator.SetFloat(vForwardHash, localVelocity.z);
            animator.SetFloat(vRightHash, localVelocity.x);

        }
        AgitationState currentAgitationState = agitationController.AgitationState;
        EnemyPerceptionState currentPerceptionState = enemyPerception.PerceptionState;

        if (currentAgitationState == AgitationState.Investigating)
        {
            animator.SetBool(isInInvestigative, true);
        }
        else if (currentAgitationState == AgitationState.Alarmed)
        {
            if (currentPerceptionState == EnemyPerceptionState.PlayerInSight)
            {
                animator.SetBool(isInCombatHash, true);
                animator.SetBool(isInInvestigative, false);
            }
            else
            {
                animator.SetBool(isInCombatHash, false);
                animator.SetBool(isInInvestigative, true);
            }
        }
        else
        {
            animator.SetBool(isInInvestigative, false);
        }


    }
}
