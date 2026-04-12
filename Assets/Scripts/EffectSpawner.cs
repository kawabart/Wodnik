using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    public SurfaceType defaultType;

    public void SpawnHit(Vector3 position, Vector3 direction, SurfaceType surfaceType)
    {
        GameObject effect = Instantiate(surfaceType.hitEffect, position, Quaternion.LookRotation(direction));
        Destroy(effect, 5f);
    }

    public void SpawnHit(Vector3 position, Vector3 direction)
    {
        SpawnHit(position, direction, defaultType);
    }

    public void SpawnHit(Vector3 position, SurfaceType surfaceType)
    {
        SpawnHit(position, Vector3.up, surfaceType);
    }
}
