using UnityEngine;

public interface ISightWatcher
{
    bool OnSightWatched(Vector3 position, DangerLevel danger, GameObject source = null, Vector3? dangerPosition = null, bool hidden = false);
}
