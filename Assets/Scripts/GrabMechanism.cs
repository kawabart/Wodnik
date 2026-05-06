using UnityEngine;

public class GrabMechanism : MonoBehaviour
{
    [SerializeField] private float minDistance = .1f;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private Grabbable grabbable;
    public float DistanceNormalized = 0;

    public void Update()
    {
        GetDistanceNormalized();
        if (DistanceNormalized >= 1) grabbable.ForceLetGo();
    }
    public float GetDistanceNormalized()
    {
        float currentDistance = Vector3.Distance(startPosition.position, endPosition.position);
        DistanceNormalized = Mathf.InverseLerp(minDistance, maxDistance, currentDistance);
        return DistanceNormalized;
    }
}
