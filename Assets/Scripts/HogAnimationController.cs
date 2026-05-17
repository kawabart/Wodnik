using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class HogAnimationController : MonoBehaviour
{
    private EnemyPerception enemyPerception;
    private AgitationController agitationController;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyPerception = GetComponent<EnemyPerception>();
        agitationController = GetComponent<AgitationController>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetFloat("Agitation", agitationController.AgitationLevel);
    }
}
