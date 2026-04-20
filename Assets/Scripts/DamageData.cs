using UnityEngine;

public struct DamageData
{
    public int amount;
    //public Vector3 point;
    //public Vector3 direction;
    public GameObject source;
    public SurfaceType overrideSurface;

    public DamageData(int amount, SurfaceType overrideSurface = null, GameObject source = null)
    {
        this.amount = amount;
        this.source = source;
        this.overrideSurface = overrideSurface;
    }
}
