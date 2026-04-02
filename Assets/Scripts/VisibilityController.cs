using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public bool Hidden;

    [SerializeField] private float bushDetectionRadius = 0.5f;
    [SerializeField] private LayerMask hidingLayers;
    [SerializeField] private Vector2[] checkOffsets;

    bool CheckIfHidden()
    {
        if (includeShadows && !IsInLight()) return true;
        int hits = 0;
        foreach (var offset in checkOffsets)
        {
            if (Physics.CheckSphere(transform.position + Vector3.forward * offset.x + Vector3.right * offset.y, bushDetectionRadius, hidingLayers)) hits++;
        }
        bool isCovered = hits >= checkOffsets.Length - 1;

        return isCovered;
    }

    #region light and shadow
    [SerializeField] private bool includeShadows = false;
    private LightController[] lightControllers;
    void CollectLights()
    {
        lightControllers = FindObjectsByType<LightController>();
    }
    bool IsInLight()
    {
        foreach (LightController light in lightControllers)
        {
            if (light.IsInLight(this.transform)) return true;
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
            Gizmos.DrawWireSphere(transform.position + Vector3.forward * offset.x + Vector3.right * offset.y, bushDetectionRadius);
        }
    }
}
