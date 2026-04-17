using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour, IDamageable
{
    public float health = 10f;
    private SurfaceType surfaceType = null;
    public bool DestroyOnDeath = true;
    public UnityEvent onDeath;
    public UnityEvent onHurt;
    void Start()
    {
        if (GetComponent<Surface>()) surfaceType = GetComponent<Surface>().type;
    }
    public void TakeDamage(int amount, GameObject source)
    {
        if (amount < 1) return;
        onHurt.Invoke();
        health -= amount;

        if (surfaceType!=null)
            EffectSpawner.Instance.SpawnHit(transform.position, surfaceType);
        else
            EffectSpawner.Instance.SpawnHit(transform.position, Vector3.up);

        if (health <= 0)
            Die();
    }

    void Die()
    {
        if (surfaceType != null)
            EffectSpawner.Instance.SpawnHit(transform.position, surfaceType);
        else
            EffectSpawner.Instance.SpawnHit(transform.position, Vector3.up);
        onDeath.Invoke();
        if (DestroyOnDeath)
            Destroy(gameObject);
    }
}
