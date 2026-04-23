using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour, IPushable
{
    public UnityEvent onPush;
    private Rigidbody rb;
    public bool rotateInPushDirection = false;

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
        if (rotateInPushDirection)
        {
            rb.MoveRotation(Quaternion.LookRotation(-force));
        }
        rb.AddForce(force, ForceMode.Impulse);
        onPush.Invoke();
    }
    public bool CanBePushed()
    {
        if (enemyController != null && enemyController.TryBlocking()) return false;
        return true;
    }
}
