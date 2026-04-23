using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPerception : MonoBehaviour
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
    private Rigidbody rigidBody;

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
    public Vector3? LastPlayerPosition = null;

    [Header("Active Senses")]
    public bool canSee = true;
    public bool canHear = true;
    public bool canTouch = true;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();

        rigidBody = GetComponent<Rigidbody>();
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
            }
            else if (PerceptionState == EnemyPerceptionState.PlayerInSight)
            {
                PerceptionState = EnemyPerceptionState.PlayerSeenRecently;
            }
            else if (LastPlayerPosition == null)
            {
                PerceptionState = EnemyPerceptionState.Idle;

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
