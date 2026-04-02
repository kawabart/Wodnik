using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public bool Hidden;// { get; private set; }

    [SerializeField] private float bushDetectionRadius = 0.5f;
    [SerializeField] private LayerMask hidingLayers;
    [SerializeField] private Vector2[] checkOffsets;

    void FixedUpdate()
    {
        Hidden = CheckVisibility();
    }

    bool CheckVisibility()
    {
        int hits = 0;
        foreach (var offset in checkOffsets)
        {
            if (Physics.CheckSphere(transform.position + Vector3.forward * offset.x + Vector3.right * offset.y, bushDetectionRadius, hidingLayers)) hits++;
        }
        bool isCovered = hits >= checkOffsets.Length-1;

        return isCovered;
    }
    private void OnDrawGizmosSelected()
    {
        if (checkOffsets == null) return;

        Gizmos.color = Color.cyan; // Dowolny kolor, ¿eby by³ widoczny
        foreach (var offset in checkOffsets)
        {
            // Rysuje kulê w pozycji gracza przesuniêtej o offset
            Gizmos.DrawWireSphere(transform.position + Vector3.forward * offset.x + Vector3.right * offset.y, bushDetectionRadius);
        }
    }
}
