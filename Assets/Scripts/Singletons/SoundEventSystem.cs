using UnityEngine;

public static class SoundEventSystem
{
    public static void Emit(Vector3 position, float radius, PercievedDangerLevels type)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius);

        foreach (var hit in hits)
        {
            var listener = hit.GetComponent<ISoundListener>();
            if (listener != null)
            {
                listener.OnSoundHeard(position, type);
            }
        }
    }
}
