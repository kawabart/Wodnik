using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public bool Hidden;

    [SerializeField] private float detectionSphereRadius = 0.5f;
    [SerializeField] private LayerMask hidingLayers;
    [SerializeField] private Vector3[] checkOffsets;

    bool CheckIfHidden()
    {
        int hits = 0;
        foreach (var offset in checkOffsets)
        {
            Vector3 positionToCheck = transform.position + offset;
            if (includeShadows && !IsInLight(positionToCheck)) hits++;
            else if (Physics.CheckSphere(positionToCheck, detectionSphereRadius, hidingLayers)) hits++;
        }
        bool isCovered = hits >= checkOffsets.Length - 1;

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
            Gizmos.DrawWireSphere(transform.position + offset, detectionSphereRadius);
        }
    }
}
