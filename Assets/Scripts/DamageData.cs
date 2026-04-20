using UnityEngine;

public struct DamageData
{
    public int Amount { get; }
    public GameObject Source { get; }
    public SurfaceType OverrideSurface { get; }

    public DamageData(int amount, SurfaceType overrideSurface = null, GameObject source = null)
    {
        this.Amount = amount;
        this.Source = source;
        this.OverrideSurface = overrideSurface;
    }
}
