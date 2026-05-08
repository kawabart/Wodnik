using System.ComponentModel.Design;
using Unity.AppUI.Redux;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class VisualSight : MonoBehaviour
{
    public bool Discovered = false;
    public DangerLevel Danger;
    public bool Hidden = false;
    public void TryDiscover(ISightWatcher watcher)
    {
        if (watcher.OnSightWatched(transform.position, Danger, this.gameObject, null, Hidden))
        {
            Discovered = true;
        }
    }
    public void ResetSight()
    {
        ResetSight(Danger);
    }
    public void ResetSight(DangerLevel dangerLevel)
    {
        Danger = dangerLevel;
        Discovered = false;
    }
}
