using UnityEngine;

[CreateAssetMenu(fileName = "Surface Type", menuName = "Scriptable Objects/SurfaceType")]
public class SurfaceType : ScriptableObject
{
    public string surfaceName;
    public GameObject hitEffect;

    public float SoundRange = .5f;
    public PercievedDangerLevels defaultDangerLevel = PercievedDangerLevels.None;
    //public AudioClip hitSound;
}
