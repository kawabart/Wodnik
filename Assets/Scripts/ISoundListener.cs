using UnityEngine;

public interface ISoundListener
{
    void OnSoundHeard(Vector3 position, DangerLevel danger, GameObject source = null, Vector3? dangerPosition = null);
}
public enum DangerLevel
{
    None,
    Noise, // any other noise
    Water, // sounds of water
    Distress, // someone screems in pain
    MaybePlayer, // hears player footsteps, something was pushed from there
    Player, //visual confirmation on player
}
