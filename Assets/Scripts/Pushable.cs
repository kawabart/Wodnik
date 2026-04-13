using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour, IPushable
{
    public UnityEvent onPush;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Push(Vector3 force)
    {
        Debug.Log("I am pushed!");
        if (rb.isKinematic) rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
        onPush.Invoke();
    }
}
