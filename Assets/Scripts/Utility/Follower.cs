using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = Vector3.zero;

    void Update()
    {
        if (Target != null) transform.position = Target.transform.position + Offset;
    }
}
