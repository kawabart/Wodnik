using UnityEngine;

public interface ISoundListener
{
    void OnSoundHeard(Vector3 position, DangerLevel danger, GameObject source = null, Vector3? dangerPosition = null);
}
