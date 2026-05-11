using UnityEngine;

//Point of interest - something that can catch enemy's attention.
public class PerceptionSight : MonoBehaviour
{
    public bool Active = false;
    public bool Discovered = false;
    public DangerLevel Danger;
    public bool Hidden = false;
    public bool OneTimeDiscovery = false;
    public float? Timeout = null;
    public bool CanBeDiscovered => Active && !Discovered;

    private void Update()
    {
        if (Timeout != null)
        {
            if (Timeout > 0)
            {
                Timeout -= Time.deltaTime;
            }
            else
            {
                Timeout = null;
                DisableSight();
            }
        }
    }

    public void TryDiscover(ISightWatcher watcher)
    {
        if (!CanBeDiscovered) return;
        if (watcher.OnSightWatched(transform.position, Danger, this.gameObject, null, Hidden))
        {
            if (OneTimeDiscovery)
                Discovered = true;
        }
    }

    public void SetSightWithTimeout(float? timeout)
    {
        SetSight(Danger, timeout);
    }

    public void SetSight()
    {
        SetSight(Danger);
    }

    public void SetSight(DangerLevel dangerLevel, float? timeout = null)
    {
        Timeout = timeout;
        Danger = dangerLevel;
        Discovered = false;
        Active = true;
    }

    public void DisableSight()
    {
        Active = false;
    }
    private void OnDrawGizmos()
    {
        if (!CanBeDiscovered)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
