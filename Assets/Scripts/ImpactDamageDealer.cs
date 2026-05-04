using UnityEngine;
using UnityEngine.Events;

public class ImpactDamageDealer : MonoBehaviour
{
    public float minVelocityToDamage = 3f;

    public float increaseVelocityNeededForPlayer = 2f;
    public int damage = 1;
    public UnityEvent onDamageDeal;

    [Header("Optional rigidbody functionality")]
    private Rigidbody rb;
    public float minSelfVelocityToDamage = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;
        if (rb != null)
        {
            float velocity = rb.linearVelocity.magnitude;
            if (velocity < minSelfVelocityToDamage)
                return;
        }

        if (impactForce < minVelocityToDamage)
            return;

        var damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (collision.collider.GetComponent<PlayerController>() && impactForce < minVelocityToDamage + increaseVelocityNeededForPlayer) return;
            damageable.TakeDamage(new DamageData(damage));
            onDamageDeal.Invoke();
        }
    }
}
