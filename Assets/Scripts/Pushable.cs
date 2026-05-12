using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Unity.Collections.AllocatorManager;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour, IPushable
{
    public UnityEvent onPush;
    private Rigidbody rb;
    public int rotateInPushDirection = 0;

    private EnemyController enemyController;
  
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        enemyController = GetComponent<EnemyController>();
    }
    
    public void Push(Vector3 force)
    {
        if (!CanBePushed())
        {
            Debug.Log("I can't be pushed!");
            return;
        }
        Debug.Log("I am pushed!");
        if (rb.isKinematic) rb.isKinematic = false;
        if (rotateInPushDirection != 0)
        {
            rb.MoveRotation(Quaternion.LookRotation(rotateInPushDirection * force));
        }
        rb.AddForce(force, ForceMode.Impulse);
        onPush.Invoke();
    }
  
    public bool CanBePushed()
    {
        // If this is a thing that can block attacks and is blocking attack, then attack failed.
        if (enemyController != null && enemyController.TryBlocking()) return false;

        // Attack succesfull.
        return true;
    }
}
