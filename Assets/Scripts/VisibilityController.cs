using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public bool Hidden { get; private set; }
    [SerializeField, Tooltip("Layers that conceal while collided with.")]
    private LayerMask hidingLayers;
    [SerializeField, Tooltip("List of points relative to object position and rotation, that are checked for collision with hiding geometry, or if theyre in shadows.")]
    private Vector3[] checkOffsets = { Vector3.zero };
    [SerializeField, Tooltip("Radius to check around points defined in checkOffets.")]
    private float detectionSphereRadius = 0.5f;
    [SerializeField, Tooltip("How many points can remain shown for the whole object to be considered hidden. Should be smaller then length of checkOffsets. Leave at 0 if you want all points to be accounted for.")]
    private int detectionPointsLeniency = 0;

    bool CheckIfHidden()
    {
        int hits = 0;
        foreach (var offset in checkOffsets)
        {
            Vector3 positionToCheck = transform.TransformPoint(offset);
            if (includeShadows && !IsInLight(positionToCheck)) hits++;
            else if (Physics.CheckSphere(positionToCheck, detectionSphereRadius, hidingLayers)) hits++;
        }
        bool isCovered = hits >= checkOffsets.Length - detectionPointsLeniency;

        return isCovered;
    }

    #region light and shadow
    [SerializeField] private bool includeShadows = false;
    private LightController[] lightControllers;
    void CollectLights()
    {
        //Can later be expanded so that some manager globally collects all lights in scene,
        //if we want to have more objects reacting to light, like hiding bodies in shadows.
        //Also should collect lights in real time, not just on start, if we want to create
        //sources of light dynamically.
        lightControllers = FindObjectsByType<LightController>();
    }
    bool IsInLight(Vector3 position)
    {
        foreach (LightController light in lightControllers)
        {
            if (light.IsInLight(position)) return true;
        }
        return false;
    }
    #endregion  

    private void Start()
    {
        CollectLights();
    }
    void FixedUpdate()
    {
        Hidden = CheckIfHidden();
    }

    private void OnDrawGizmosSelected()
    {
        if (checkOffsets == null) return;

        Gizmos.color = Color.cyan;
        foreach (var offset in checkOffsets)
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(offset), detectionSphereRadius);
        }
    }
}
