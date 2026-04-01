using UnityEngine;

public class EnemyTesting : MonoBehaviour
{
    public Animator animator;
    public float agitation = 0;
    public float speed = 1;
    public bool attack = false;
    void Start()
    {
        
    }

    void Update()
    {
        animator.SetBool("attack", attack);
        animator.SetFloat("speed", speed);
        animator.SetFloat("agitation", agitation);
        if (attack)
        {
            attack = false;
        }
    }
}
