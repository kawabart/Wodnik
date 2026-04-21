using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(BehaviorGraphAgent))]
public class EnemyAnimationController : MonoBehaviour
{

    public void ChangeCombatState()
    {
        if (behaviorAgent.BlackboardReference.GetVariableValue<EnemyPerceptionState>("EnemyPerceptionState", out var currentPerceptionState))
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
            Debug.LogWarning("Could not find EnemyPerceptionState on the Blackboard!");
        }
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
    private NavMeshAgent navMeshAgent;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        enemyController = GetComponent<EnemyController>();
        agitationController = GetComponent<AgitationController>();
        animator = GetComponentInChildren<Animator>();
        meleeDamageDealer = GetComponent<MeleeDamageDealer>();
        isInCombatHash = Animator.StringToHash("IsInCombat");
        isInInvestigative = Animator.StringToHash("IsInInvestigative");
        attackHash = Animator.StringToHash("Attack");
        speedHash = Animator.StringToHash("Speed");
    }

    void Update()
    {
        if (navMeshAgent.enabled)
        {
            float speed = navMeshAgent.velocity.magnitude;
            animator.SetFloat(speedHash, speed);
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
