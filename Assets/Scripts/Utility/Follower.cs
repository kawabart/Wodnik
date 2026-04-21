using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private Vector3 Offset = Vector3.zero;
    public float SmoothTime = 0.3f;
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField] private bool lateUpdate = false;
    void LerpPosition()
    {
        if (Target == null) return;

        Vector3 targetPosition = Target.position + Offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            SmoothTime
        );
    }
    void Update()
    {
        if (!lateUpdate)
            LerpPosition();

    }
    void LateUpdate()
    {
        if (lateUpdate)
            LerpPosition();
    }
}
