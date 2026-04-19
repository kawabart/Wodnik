using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent))]
public class AnimationController : MonoBehaviour
{

    public void changeCombatState()
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

    private BehaviorGraphAgent behaviorAgent;
    private Animator animator;
    private int isInCombatHash;
    private int isInInvestigative;
    void Start()
    {
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        animator = GetComponentInChildren<Animator>();
        isInCombatHash = Animator.StringToHash("IsInCombat");
        isInInvestigative = Animator.StringToHash("IsInInvestigative");
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
                changeCombatState();
            } else
            {
                animator.SetBool(isInInvestigative, false);
            }
        }
    }
}
