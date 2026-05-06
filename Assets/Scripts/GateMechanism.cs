using UnityEngine;

public class GateMechanism : MonoBehaviour
{
     private Vector3 startingPoint;
    [SerializeField] private Vector3 offset = new Vector3(0,1,0);
    [SerializeField] private GrabMechanism grabMechanism;
    private Rigidbody rb;

    private void Start()
    {
        startingPoint = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(Vector3.Lerp(startingPoint, startingPoint + offset, grabMechanism.DistanceNormalized));
    }
}
