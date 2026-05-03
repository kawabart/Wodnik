using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ImpactDamageDealer : MonoBehaviour
{
    public float minVelocityToDamage = 3f;
    public float minSelfVelocityToDamage = 0f;
    public float increaseVelocityNeededForPlayer = 2f;
    public int damage = 1;
    public UnityEvent onDamageDeal;
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
 
    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;
        float velocity = rb.linearVelocity.magnitude;

        if (impactForce < minVelocityToDamage)
            return;

        if (velocity < minSelfVelocityToDamage)
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
