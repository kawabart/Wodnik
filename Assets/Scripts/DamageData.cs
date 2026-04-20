using UnityEngine;

public struct DamageData
{
    public int amount;
    //public Vector3 point;
    //public Vector3 direction;
    public GameObject source;
    public SurfaceType overrideSurface;

    public DamageData(int amount)
    {
        this.amount = amount;
        this.source = null;
        this.overrideSurface = null;
    }
    public DamageData(int amount, SurfaceType overrideSurface)
    {
        this.amount = amount;
        this.source = null;
        this.overrideSurface = overrideSurface;
    }
    
    public DamageData(int amount, SurfaceType overrideSurface, GameObject source)
    {
        this.amount = amount;
        this.source = source;
        this.overrideSurface = overrideSurface;
    }
    public DamageData newData(int amount = 0, SurfaceType overrideSurface = null, GameObject source = null)
    {
        return new DamageData(amount, overrideSurface, source);
    }
}
