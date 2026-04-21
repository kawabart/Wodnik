using UnityEngine;

public class AreaDamageDealer : MonoBehaviour
{
    public int damage = 10;
    public bool affectPlayer = false;
    public bool affectOnlyDowned = false;
    public SurfaceType overrideSurface;

    void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if (damageable == null) return;

        if (!affectPlayer && other.GetComponent<PlayerController>()) return;

        var enemy = other.GetComponent<EnemyController>();
        if (affectOnlyDowned && enemy != null && enemy.CurrentState != EnemyState.Downed)
            return;

        damageable.TakeDamage(new DamageData(damage, overrideSurface));
    }
}
