using UnityEngine;

public class ImpactDamageDealer : MonoBehaviour
{
    public float minVelocityToDamage = 5f;
    public float damageMultiplier = 2f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce < minVelocityToDamage)
            return;

        float damage = impactForce * damageMultiplier;

        var damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}
