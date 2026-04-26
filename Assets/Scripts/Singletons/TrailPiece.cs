using UnityEngine;

public class TrailPiece : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform nextPosition;
    public void EmmitNextPosition()
    {
        SoundEventSystem.Emit(transform.position, 1, DangerLevel.Distress, null, nextPosition.position);
    }
    public void EmmitSelfPosition()
    {
        SoundEventSystem.Emit(transform.position, 1, DangerLevel.Noise, null, transform.position);
    }
}
