using UnityEngine;

[CreateAssetMenu(fileName = "Surface Type", menuName = "Scriptable Objects/SurfaceType")]
public class SurfaceType : ScriptableObject
{
    public string surfaceName;
    public GameObject hitEffect;
    //public AudioClip hitSound;
}
