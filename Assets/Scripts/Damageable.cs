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
    public void TakeDamage(DamageData damageData)
    {
        int amount = damageData.Amount;
        onHurt.Invoke();
        health -= amount;
        SurfaceType currentSurface = surfaceType;
        if (damageData.OverrideSurface != null)
            currentSurface = damageData.OverrideSurface;

        if (surfaceType != null)
            EffectSpawner.Instance.SpawnHit(transform.position, currentSurface);
        else
            EffectSpawner.Instance.SpawnHit(transform.position, Vector3.up);

        SoundEventSystem.Emit(transform.position, currentSurface.SoundRange, currentSurface.defaultDangerLevel, this.gameObject);

        if (health <= 0)
            Die(currentSurface);
    }

    public void Die(SurfaceType currentSurface = null)
    {
        if (currentSurface != null)
            EffectSpawner.Instance.SpawnHit(transform.position, currentSurface);
        else
            EffectSpawner.Instance.SpawnHit(transform.position, Vector3.up);
        onDeath.Invoke();
        if (DestroyOnDeath)
            Destroy(gameObject);
    }
}
