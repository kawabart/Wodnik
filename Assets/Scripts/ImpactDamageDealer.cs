using UnityEngine;
using UnityEngine.Events;

public class ImpactDamageDealer : MonoBehaviour
{
    public float minVelocityToDamage = 3f;

    public float increaseVelocityNeededForPlayer = 2f;
    public int damage = 1;

    public UnityEvent onDamageDeal;

    private Rigidbody rb;
    public float minSelfVelocityToDamage = 0f;

    private float damageOverrideTimer = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (damageOverrideTimer > 0)
            damageOverrideTimer -= Time.fixedDeltaTime;
    }
    public void OverrideDamageForTime(float time = .2f)
    {
        damageOverrideTimer = time;
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

        if (impactForce < minVelocityToDamage && damageOverrideTimer <=0)
            return;

        if (collision.collider.GetComponent<PlayerController>() && impactForce < minVelocityToDamage + increaseVelocityNeededForPlayer) 
            return;

        var damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            
            damageable.TakeDamage(new DamageData(damage));
            onDamageDeal.Invoke();
            damageOverrideTimer = 0;
        }
    }
}
