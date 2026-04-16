using UnityEngine;

[CreateAssetMenu(fileName = "AgitationStateConfig", menuName = "Scriptable Objects/AgitationStateConfig")]
public class AgitationStateConfig : ScriptableObject
{
    public float AgitationLevel = 30;
    public float MoveSpeed = 3;

    [Tooltip("How fast agitation level increases when the enemy sees the player, per second.")]
    public float AgitationPositiveRate = 50;

    [Tooltip("How fast agitation level decreases when the enemy doesn't see the player, per second.")]
    public float AgitationNegativeRate = 10;
}
