using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform target;
    [SerializeField] private Transform startPoint;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (startPoint == null)
            startPoint = transform;
    }
    private void Update()
    {
        
        lineRenderer.SetPosition(0, startPoint.position);

        lineRenderer.SetPosition(1, target.position);
    }
}
