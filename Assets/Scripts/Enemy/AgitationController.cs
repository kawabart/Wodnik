using UnityEngine;

[RequireComponent(typeof(EnemyPerception))]
public class AgitationController : MonoBehaviour
{
    public float MinAgitation = 0, MaxAgitation = 100;

    public AgitationStateConfig RelaxedConfig, InvestigatingConfig, AlarmedConfig;

    public AgitationStateConfig CurrentAgitationConfig;

    private PerceptionSight perceptionSight;
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
        perceptionSight = GetComponent<PerceptionSight>();
        UpdateAgitation();
    }
    private bool isInShock = false;
    private void UpdateAgitation()
    {
        if (AgitationLevel > AlarmedConfig.AgitationLevel || AgitationLevel > RelaxedConfig.AgitationLevel && CurrentAgitationConfig == AlarmedConfig)
        {
            if (AgitationState != AgitationState.Alarmed)
            {
                isInShock = true;
                shockTimer = shockTime;
                AgitationState = AgitationState.Alarmed;
                //Example of enemy informing other enemies about the location of the problem

                perceptionSight.SetSight(DangerLevel.Distress);
            }
            if (shockTimer > 0)
            {
                shockTimer -= Time.deltaTime;
            }
            else if (isInShock)
            {
                isInShock = false;
                SoundEventSystem.Emit(transform.position, 1f, DangerLevel.MaybePlayer, this.gameObject, perception.LastPlayerPosition);
                SoundEventSystem.Emit(transform.position, 3.5f, DangerLevel.Distress, this.gameObject, perception.LastPlayerPosition);      
            }
            CurrentAgitationConfig = AlarmedConfig;

        }
        else if (AgitationLevel > InvestigatingConfig.AgitationLevel)
        {
            if (AgitationState != AgitationState.Investigating)
            {
                perceptionSight.DisableSight();
            }
            CurrentAgitationConfig = InvestigatingConfig;

            AgitationState = AgitationState.Investigating;

        }
        else
        {
            if (AgitationState != AgitationState.Relaxed)
            {
                perceptionSight.DisableSight();
            }
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
