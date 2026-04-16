using UnityEngine;

[RequireComponent(typeof(EnemyPerception))]
public class AgitationController : MonoBehaviour
{
    public float MinAgitation = 0, MaxAgitation = 100;

    [Tooltip("How fast agitation level increases when the enemy sees the player, per second.")]
    public float AgitationPositiveRate = 50;

    [Tooltip("How fast agitation level decreases when the enemy doesn't see the player, per second.")]
    public float AgitationNegativeRate = 10;

    public AgitationStateConfig RelaxedConfig, InvestigatingConfig, AlarmedConfig;

    private EnemyPerception perception;
    public float AgitationLevel = 0;
    public AgitationState AgitationState = AgitationState.Relaxed;

    public float SuggestedSpeed = 5;

    void Start()
    {
        perception = GetComponent<EnemyPerception>();
    }

    void Update()
    {
        if (perception.PerceptionState == EnemyPerceptionState.PlayerInSight)
        {
            UpdateAgitation(AgitationPositiveRate * Time.deltaTime);
        }
        else if (perception.PerceptionState == EnemyPerceptionState.PlayerSeenRecently)
        {
            UpdateAgitation(-AgitationNegativeRate * Time.deltaTime);
        }
    }

    private void UpdateAgitation(float change)
    {
        AgitationLevel = Mathf.Clamp(AgitationLevel + change, MinAgitation, MaxAgitation);
        if (AgitationLevel > AlarmedConfig.AgitationLevel)
        {
            AgitationState = AgitationState.Alarmed;
            SuggestedSpeed = AlarmedConfig.MoveSpeed;
        }
        else if (AgitationLevel > InvestigatingConfig.AgitationLevel)
        {
            AgitationState = AgitationState.Investigating;
            SuggestedSpeed = InvestigatingConfig.MoveSpeed;
        }
        else
        {
            AgitationState = AgitationState.Relaxed;
            SuggestedSpeed = RelaxedConfig.MoveSpeed;
        }
    }
}
