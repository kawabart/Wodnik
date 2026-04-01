using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private Vector3 Offset = Vector3.zero;
    [SerializeField] private float SmoothTime = 0.3f;
    private Vector3 _currentVelocity = Vector3.zero;
    void Update()
    {
        if (Target != null)
        {
            Vector3 targetPosition = Target.position + Offset;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref _currentVelocity,
                SmoothTime
            );
        }

    }
}
