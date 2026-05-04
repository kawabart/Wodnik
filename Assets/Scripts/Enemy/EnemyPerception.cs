using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPerception : MonoBehaviour, ISoundListener
{
    [SerializeField]
    private EnemyController enemyController;
    private AgitationStateConfig CurrentAgitationConfig
    {
        get
        {
            return enemyController.CurrentAgitationConfig;
        }
    }
  
    [SerializeField]
    private PlayerController player = null;
    [SerializeField]
    private Transform eyesPosition;
    [SerializeField]
    private LayerMask percivedLayerMask;
    private Rigidbody playerRigidBody;
    [SerializeField]
    public EnemyPerceptionState PerceptionState = EnemyPerceptionState.Idle;

    private float distanceMultiplierNormalized = 0;

    [Header("Sight values modified by Scriptable")]
    public float SightDistance = 2;
    public float SightFOVDegrees = 45;
    [Tooltip("Grace period in which enemy still has player in sight, even if they can't physically see them.")]
    public float PredictPlayerPositionTime = 1;
    public float PredictPlayerPositionTimer = 0;
    [Tooltip("Distance in which the enemy detects player, even if they're hidden.")]
    public float NoticeHiddenPlayerDistance = .5f;

    [SerializeField]
    DangerLevel percievedDangerLevel = DangerLevel.None;

    [SerializeField]
    public Vector3? LastPlayerPosition = null;

    [Header("Active Senses")]
    public bool canSee = true;
    public bool canHear = true;
    public bool canTouch = true;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();

        player = (PlayerController)FindAnyObjectByType(typeof(PlayerController));
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (player != null)
        {
            if (KnowsPlayerPosition())
            {
                LastPlayerPosition = player.transform.position;
                PerceptionState = EnemyPerceptionState.PlayerInSight;
                percievedDangerLevel = DangerLevel.Player;
            }
            else if (PerceptionState == EnemyPerceptionState.PlayerInSight)
            {
                if (percievedDangerLevel == DangerLevel.Player)
                    percievedDangerLevel = DangerLevel.MaybePlayer;
                PerceptionState = EnemyPerceptionState.PlayerSeenRecently;
            }
            else if (LastPlayerPosition == null)
            {
                PerceptionState = EnemyPerceptionState.Idle;
                percievedDangerLevel = DangerLevel.None;

            }
        }
        UpdateValuesFromScriptable();

        if (PerceptionState == EnemyPerceptionState.PlayerInSight)
        {
            enemyController.IncreaseAgitation(CurrentAgitationConfig.VisibilityCurve.Evaluate(distanceMultiplierNormalized));
        }
        else if (PerceptionState == EnemyPerceptionState.Idle)
        {
            enemyController.DecreaseAgitation();
        }
    }
 
    void UpdateValuesFromScriptable()
    {
        if (CurrentAgitationConfig == null) return;

        PredictPlayerPositionTime = CurrentAgitationConfig.PredictPlayerPositionTime;
        NoticeHiddenPlayerDistance = CurrentAgitationConfig.NoticeHiddenPlayerDistance;
        SightDistance = CurrentAgitationConfig.SightDistance;
        SightFOVDegrees = CurrentAgitationConfig.SightFOVDegrees;
    }

    private bool KnowsPlayerPosition()
    {
        PredictPlayerPositionTimer -= Time.deltaTime;
        if (DetectPlayer())
        {
            PredictPlayerPositionTimer = PredictPlayerPositionTime;
            return true;
        }
        if (PredictPlayerPositionTimer > 0) return true;
        else return false;
    }
 
    public void OnSoundHeard(Vector3 position, DangerLevel danger, GameObject source = null, Vector3? dangerPosition = null)
    {
        if (!canHear) return;
        if (source == this.gameObject) return;
        if (percievedDangerLevel > danger) return;
        if (PerceptionState == EnemyPerceptionState.PlayerInSight) return;

        if (dangerPosition == null) dangerPosition = position;
        float agitationIncrement = 0;
        float maxAgitationFromDanger = 100;
        //To do: move this functionality and values out of this method, so that they can be used elsewhere, not only by sound.
        switch (danger)
        {
            case DangerLevel.Noise:
                agitationIncrement = 20;
                maxAgitationFromDanger = 50;
                break;
            case DangerLevel.Water:
                agitationIncrement = 40;
                break;
            case DangerLevel.Distress:
                agitationIncrement = 50;
                maxAgitationFromDanger = 90;
                break;
            case DangerLevel.MaybePlayer:
                agitationIncrement = 100;
                break;
            case DangerLevel.Player:
                agitationIncrement = 100;
                break;
        }
        enemyController.IncreaseAgitation(agitationIncrement, false, false, maxAgitationFromDanger);
        PerceptionState = EnemyPerceptionState.PlayerSeenRecently;
        percievedDangerLevel = danger;
        LastPlayerPosition = dangerPosition;

    }
 
    public void ActivateSenses()
    {
        canSee = true;
        canHear = true;
        canTouch = true;
    }

    public void DectivateSenses()
    {
        canSee = false;
        canHear = false;
        canTouch = false;
    }

    private bool DetectPlayer()
    {
        if (!canSee) return false;
        RaycastHit hit;
        Vector3 targetPosition = playerRigidBody.transform.position + Vector3.up * .15f;

        float sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
        if (sqrDistance > SightDistance * SightDistance)
        {
            distanceMultiplierNormalized = 0;
            return false;
        }
        float distance = (player.transform.position - transform.position).magnitude;
        distanceMultiplierNormalized = SightDistance == 0 ? 0 : 1 - distance / SightDistance;
        var direction = targetPosition - eyesPosition.position;
        var angle = Vector3.Angle(transform.forward, direction);
        if (angle > SightFOVDegrees)
        {
            return false;
        }


        if (sqrDistance < NoticeHiddenPlayerDistance * NoticeHiddenPlayerDistance) return true;

        if (player.Hidden)
        {
            return false;
        }
        if (Physics.Raycast(eyesPosition.position, direction, out hit, SightDistance, percivedLayerMask))
        {
            Debug.DrawRay(eyesPosition.position, direction.normalized * SightDistance, Color.green);

            if (hit.collider.gameObject == player.gameObject)
            {
                Debug.DrawRay(eyesPosition.position, direction.normalized * SightDistance, Color.yellow);
                return true;
            }
        }
        else
        {
            Debug.DrawRay(eyesPosition.position, direction.normalized * SightDistance, Color.red);
        }

        return false;
    }
}
