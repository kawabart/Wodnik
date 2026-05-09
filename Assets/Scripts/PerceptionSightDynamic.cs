using Unity.VisualScripting;
using UnityEngine;

public class PerceptionSightDynamic : PerceptionSight
{
    private Rigidbody rb;
    private float minVelocity = 1f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (rb == null) return;
        if (rb.linearVelocity.sqrMagnitude > minVelocity * minVelocity)
        {
            SetSightWithTimeout(1f);
            OneTimeDiscovery = true;
        }
    }
}
