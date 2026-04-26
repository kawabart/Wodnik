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

    private float shockTimer = 0;
    [SerializeField, Tooltip("Grace period where enemy is still vulnerable after entering Alerted state (in seconds)")]
    private float shockTime = 1f;
    public bool IsShocked()
    {
        return shockTimer > 0;
    }
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
                shockTimer = shockTime;
                AgitationState = AgitationState.Alarmed;
                //Example of enemy informing other enemies about the location of the problem
                SoundEventSystem.Emit(transform.position, 3.5f, DangerLevel.Distress, this.gameObject, perception.LastPlayerPosition);
            }
            if (shockTimer > 0)
                shockTimer -= Time.deltaTime;
            CurrentAgitationConfig = AlarmedConfig;

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

    /// <summary>
    /// Increases entity's agitation.
    /// </summary>
    /// <param name="input">Base increase to agitation.</param>
    /// <param name="affectedByAgitationState">Should values from current agitation config should affect this increase?</param>
    /// <param name="continous">Should this increase be affected by delta time (continous), or is it just one time input?.</param>
    /// <param name="maxAgitationFromThis">This input won't increase agitation above said number.</param>
    public void IncreaseAgitation(float input, bool affectedByAgitationState = true, bool continous = true, float maxAgitationFromThis = 100)
    {
        if (AgitationLevel > maxAgitationFromThis) return;
        if (continous) input *= Time.deltaTime;
        if (affectedByAgitationState) input *= CurrentAgitationConfig.AgitationPositiveRate;
        AgitationLevel = Mathf.Min(AgitationLevel + input, maxAgitationFromThis);
        UpdateAgitation();
    }
    public void DecreaseAgitation()
    {
        var change = CurrentAgitationConfig.AgitationNegativeRate * Time.deltaTime;
        AgitationLevel = Mathf.Max(MinAgitation, AgitationLevel - change);
        UpdateAgitation();
    }
}
