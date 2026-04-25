using UnityEngine;
using UnityEngine.Rendering;

public static class SoundEventSystem
{
    public static void Emit(Vector3 position, float radius, PercievedDangerLevels type, GameObject source = null, Vector3 ? dangerPosition = null)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius);
        if (dangerPosition == null) dangerPosition = position;
        foreach (var hit in hits)
        {
            if (hit.gameObject == source) continue;
            var listener = hit.GetComponent<ISoundListener>();
            if (listener != null)
            {
                listener.OnSoundHeard(position, type, source=null, dangerPosition=null);
            }
        }
    }
}
