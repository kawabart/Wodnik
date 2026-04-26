using UnityEngine;

[CreateAssetMenu(fileName = "Surface Type", menuName = "Scriptable Objects/SurfaceType")]
public class SurfaceType : ScriptableObject
{
    public string surfaceName;
    public GameObject hitEffect;

    public float SoundRange = .5f;
    public DangerLevel defaultDangerLevel = DangerLevel.None;
    //public AudioClip hitSound;
}
