using UnityEngine;

[RequireComponent(typeof(EnemyPerception))]
public class AgitationController : MonoBehaviour
{
    public float MinAgitation = 0, MaxAgitation = 100;

    public AgitationStateConfig RelaxedConfig, InvestigatingConfig, AlarmedConfig;

    public AgitationStateConfig CurrentAgitationConfig;

    private EnemyPerception perception;
    public float AgitationLevel = 0;
    public AgitationState AgitationState = AgitationState.Relaxed;

    public float SuggestedSpeed = 5;

    void Start()
    {
        perception = GetComponent<EnemyPerception>();
        UpdateAgitation();
    }

    void Update()
    {
        if (perception.PerceptionState == EnemyPerceptionState.PlayerInSight)
        {
            IncreaseAgitation();
        }
        else if (perception.PerceptionState == EnemyPerceptionState.Idle)
        {
            DecreaseAgitation();
        }
    }

    private void UpdateAgitation()
    {
        if (AgitationLevel > AlarmedConfig.AgitationLevel || AgitationLevel > RelaxedConfig.AgitationLevel && CurrentAgitationConfig == AlarmedConfig)
        {
            CurrentAgitationConfig = AlarmedConfig;
            AgitationState = AgitationState.Alarmed;
        }
        else if (AgitationLevel > InvestigatingConfig.AgitationLevel)
        {
            CurrentAgitationConfig = InvestigatingConfig;
            AgitationState = AgitationState.Investigating;
        }
        else
        {
            CurrentAgitationConfig = RelaxedConfig;
            AgitationState = AgitationState.Relaxed;
        }
        SuggestedSpeed = CurrentAgitationConfig.MoveSpeed;
    }

    private void IncreaseAgitation()
    {
        var change = CurrentAgitationConfig.AgitationPositiveRate * Time.deltaTime;
        AgitationLevel = Mathf.Min(AgitationLevel + change, MaxAgitation);
        UpdateAgitation();
    }

    private void DecreaseAgitation()
    {
        var change = CurrentAgitationConfig.AgitationNegativeRate * Time.deltaTime;
        AgitationLevel = Mathf.Max(MinAgitation, AgitationLevel - change);
        UpdateAgitation();
    }
}
