using UnityEngine;

[RequireComponent(typeof(EnemyPerception))]
public class AgitationController : MonoBehaviour
{

    private const float MIN_AGITATION = 0, MAX_AGITATION = 100;

    public float InvestigatingAgitation = 30;
    public float AlarmedAgitation = 99;

    public float RelaxedSpeed = 3;
    public float InvestigatingSpeed = 4;
    public float AlarmedSpeed = 5;

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
            UpdateAgitation(100 * Time.deltaTime);
        }
        else if (perception.PerceptionState == EnemyPerceptionState.PlayerSeenRecently)
        {
            UpdateAgitation(-10 * Time.deltaTime);
        }
    }

    private void UpdateAgitation(float change)
    {
        AgitationLevel = Mathf.Clamp(AgitationLevel + change, MIN_AGITATION, MAX_AGITATION);
        if (AgitationLevel < InvestigatingAgitation)
        {
            AgitationState = AgitationState.Relaxed;
            SuggestedSpeed = RelaxedSpeed;
        }
        else if (AgitationLevel < AlarmedAgitation)
        {
            AgitationState = AgitationState.Investigating;
            SuggestedSpeed = InvestigatingSpeed;
        }
        else
        {
            AgitationState = AgitationState.Alarmed;
            SuggestedSpeed = AlarmedSpeed;
        }
    }
}
