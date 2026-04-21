using Unity.Behavior;
using UnityEngine;

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

    private BehaviorGraphAgent behaviorAgent;
    private Animator animator;
    private MeleeDamageDealer meleeDamageDealer;
    private int isInCombatHash;
    private int isInInvestigative;
    private int attackHash;
    void Start()
    {
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        animator = GetComponentInChildren<Animator>();
        meleeDamageDealer = GetComponent<MeleeDamageDealer>();
        isInCombatHash = Animator.StringToHash("IsInCombat");
        isInInvestigative = Animator.StringToHash("IsInInvestigative");
        attackHash = Animator.StringToHash("Attack");
    }

    void Update()
    {
        if (behaviorAgent.BlackboardReference.GetVariableValue<AgitationState>("Agitation State", out var currentAgitationState))
        {
            if (currentAgitationState == AgitationState.Investigating)
            {
                animator.SetBool(isInInvestigative, true);
            }
            else if (currentAgitationState == AgitationState.Alarmed)
            {
                ChangeCombatState();
            } else
            {
                animator.SetBool(isInInvestigative, false);
            }
        }
    }
}
