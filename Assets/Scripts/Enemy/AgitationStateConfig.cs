using UnityEngine;

[CreateAssetMenu(fileName = "AgitationStateConfig", menuName = "Scriptable Objects/AgitationStateConfig")]
public class AgitationStateConfig : ScriptableObject
{
    [Header("Agitation")]
    public float AgitationLevel = 30;
    public float MoveSpeed = 3;

    [Tooltip("How fast agitation level increases when the enemy sees the player, per second.")]
    public float AgitationPositiveRate = 50;

    [Tooltip("How fast agitation level decreases when the enemy doesn't see the player, per second.")]
    public float AgitationNegativeRate = 10;

    [Header("Perception")]
    public float SightDistance = 2;
    public float SightFOVDegrees = 45;

    [Tooltip("Grace period in which enemy still has player in sight, even if they can't physically see them.")]
    public float PredictPlayerPositionTime = .1f;

    [SerializeField, Tooltip("Distance in which the enemy detects player, even if they're hidden.")]
    public float NoticeHiddenPlayerDistance = .5f;

    [SerializeField, Tooltip("Curve that multiplies AgitationPositiveRate depending on disance.")]
    public AnimationCurve VisibilityCurve = AnimationCurve.Constant(0, 1, 1);
}
