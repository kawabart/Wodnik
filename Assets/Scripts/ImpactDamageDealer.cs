using UnityEngine;

public class ImpactDamageDealer : MonoBehaviour
{
    public float minVelocityToDamage = 5f;
    public int damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce < minVelocityToDamage)
            return;

        var damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}
