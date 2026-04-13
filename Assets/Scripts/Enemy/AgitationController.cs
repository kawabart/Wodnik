using UnityEngine;

[RequireComponent(typeof(EnemyPerception))]
public class AgitationController : MonoBehaviour
{

    private const float MIN_AGITATION = 0, MAX_AGITATION = 100;
    private const float UNBOTHERED_AGITATION = 1, ANGRY_AGITATION = 99;
    private EnemyPerception perception;
    public float AgitationLevel = 0;
    public AgitationState AgitationState = AgitationState.Unbothered;

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
        if (AgitationLevel < UNBOTHERED_AGITATION)
        {
            AgitationState = AgitationState.Unbothered;
        }
        else if (AgitationLevel < ANGRY_AGITATION)
        {
            AgitationState = AgitationState.Irritated;
        }
        else
        {
            AgitationState = AgitationState.Angry;
        }
    }

}
