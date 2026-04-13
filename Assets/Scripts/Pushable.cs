using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour, IPushable
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Push(Vector3 force)
    {
        Debug.Log("I am pushed!");
        rb.AddForce(force, ForceMode.Impulse);
    }
}
