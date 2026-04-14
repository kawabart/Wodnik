using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPerception : MonoBehaviour
{
    [SerializeField]
    private PlayerController player = null;
    [SerializeField]
    private LayerMask percivedLayerMask;
    private Rigidbody playerRigidBody;
    [SerializeField]
    public EnemyPerceptionState PerceptionState = EnemyPerceptionState.Idle;
    private Rigidbody rigidBody;

    public float SightDistance = 2;
    public float SightFOVDegrees = 45;
    [SerializeField]
    public Vector3? LastPlayerPosition = null;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        player = (PlayerController)FindAnyObjectByType(typeof(PlayerController));
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player != null)
        {
            if (DetectPlayer())
            {
                LastPlayerPosition = player.transform.position;
                PerceptionState = EnemyPerceptionState.PlayerInSight;
            }
            else if (PerceptionState == EnemyPerceptionState.PlayerInSight)
            {
                PerceptionState = EnemyPerceptionState.PlayerSeenRecently;
            }
        }
    }

    private bool DetectPlayer()
    {
        RaycastHit hit;
        var direction = playerRigidBody.worldCenterOfMass - rigidBody.worldCenterOfMass;
        direction.y = 0;
        var angle = Vector3.Angle(transform.forward, direction);
        if (angle > SightFOVDegrees)
        {
            return false;
        }
        if (player.Hidden)
        {
            return false;
        }
        if (Physics.Raycast(rigidBody.worldCenterOfMass, direction, out hit, SightDistance, percivedLayerMask))
        {
            if (hit.collider.gameObject == player.gameObject)
            {
                Debug.DrawRay(rigidBody.worldCenterOfMass, direction, Color.yellow);
                return true;
            }
        }

        return false;
    }
}
