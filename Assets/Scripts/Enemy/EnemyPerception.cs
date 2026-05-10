using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPerception : MonoBehaviour, ISoundListener, ISightWatcher
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

    [SerializeField]
    private float sensesTick = .5f;
    private float sensesTickTimer = 0;

    private GameObject lastInvestigatedObject = null;
    void Start()
    {
        sensesTickTimer = sensesTick * Random.value;
        enemyController = GetComponent<EnemyController>();

        player = (PlayerController)FindAnyObjectByType(typeof(PlayerController));
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        sensesTickTimer += Time.deltaTime;
        while (sensesTickTimer > sensesTick)
        {
            ScanForSights();
            sensesTickTimer -= sensesTick;
        }

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
    public void ScanForSights()
    {
        if (!canSee) return;
        Collider[] hits = Physics.OverlapSphere(
        eyesPosition.position,
        SightDistance,
        percivedLayerMask
        );
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<PerceptionSight>(out PerceptionSight sight))
            {
                if (sight.gameObject != gameObject)
                    sight.TryDiscover(this);
            }
        }
    }

    public bool OnSightWatched(Vector3 position, DangerLevel danger, GameObject source, Vector3? dangerPosition = null, bool hidden = false)
    {
        if (!canSee) return false;
        if (!DetectWithSight(source, hidden)) return false;
        ReactToDanger(position, danger, source, dangerPosition);
        return true;
    }

    public void OnSoundHeard(Vector3 position, DangerLevel danger, GameObject source = null, Vector3? dangerPosition = null)
    {
        if (!canHear) return;
        ReactToDanger(position, danger, source, dangerPosition);
    }

    public bool ReactToDanger(Vector3 position, DangerLevel danger, GameObject source = null, Vector3? dangerPosition = null)
    {

        if (source == this.gameObject) return false;
        if (percievedDangerLevel > danger) return false;
        if (PerceptionState == EnemyPerceptionState.PlayerInSight) return false;

        if (dangerPosition == null) dangerPosition = position;
        float agitationIncrement = 0;
        float maxAgitationFromDanger = 100;
        lastInvestigatedObject = source;
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
        return true;
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
        return DetectWithSight(player.gameObject, player.Hidden);
    }
    private bool DetectWithSight(GameObject source, bool hidden = false)
    {
        if (!canSee) return false;
        RaycastHit hit;
        Vector3 targetPosition = source.transform.position + Vector3.up * .15f;

        float sqrDistance = (source.transform.position - transform.position).sqrMagnitude;
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

        if (hidden)
        {
            return false;
        }
        if (Physics.Raycast(eyesPosition.position, direction, out hit, SightDistance, percivedLayerMask))
        {
            Debug.DrawRay(eyesPosition.position, direction.normalized * SightDistance, Color.green);

            if (hit.collider.gameObject == source)
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
