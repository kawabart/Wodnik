using UnityEngine;
using UnityEngine.Events;

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
        return enemyController == null || !enemyController.TryBlocking();
    }
}
