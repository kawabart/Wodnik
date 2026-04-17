using UnityEngine;

public class ImpactDamageDealer : MonoBehaviour
{
    public float minVelocityToDamage = 3f;
    public float increaseVelocityNeededForPlayer = 2f;
    public int damage = 1;
    

    void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce < minVelocityToDamage)
            return;

        var damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (collision.collider.GetComponent<PlayerController>() && impactForce < minVelocityToDamage + increaseVelocityNeededForPlayer) return;
            damageable.TakeDamage(damage,gameObject);
        }
    }
}
