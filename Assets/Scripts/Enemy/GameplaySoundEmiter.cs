using UnityEngine;

public class GameplaySoundEmiter : MonoBehaviour
{
    [SerializeField] private float soundRange = 1;
    [SerializeField] private DangerLevel defaultDangerLevel = DangerLevel.None;

    public void EmitSound()
    {
        SoundEventSystem.Emit(this.transform.position, soundRange, defaultDangerLevel, this.gameObject);
    }
}
