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

    private void UpdateAgitation()
    {
        if (AgitationLevel > AlarmedConfig.AgitationLevel || AgitationLevel > RelaxedConfig.AgitationLevel && CurrentAgitationConfig == AlarmedConfig)
        {
            if (AgitationState != AgitationState.Alarmed)
            {
                //SoundEventSystem.Emit(transform.position, 3, PercievedDangerLevels.Distress,this.gameObject);
            }
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

    public void IncreaseAgitation(float multiplier, float maxAgitationFromThis = 100)
    {
        if (AgitationLevel > maxAgitationFromThis) return;
        var change = CurrentAgitationConfig.AgitationPositiveRate * multiplier * Time.deltaTime;
        AgitationLevel = Mathf.Min(AgitationLevel + change, maxAgitationFromThis);
        UpdateAgitation();
    }

    public void DecreaseAgitation()
    {
        var change = CurrentAgitationConfig.AgitationNegativeRate * Time.deltaTime;
        AgitationLevel = Mathf.Max(MinAgitation, AgitationLevel - change);
        UpdateAgitation();
    }
}
