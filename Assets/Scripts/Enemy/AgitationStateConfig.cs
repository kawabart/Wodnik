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

    //for future - wasn't sure how to connect them properly, didnt want to make spaghetti
    [Tooltip("Grace period in which enemy still has player in sight, even if they can't physically see them.")]
    public float PredictPlayerPositionTime = .1f;
}
