using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Camera mirrorCamera;
    [SerializeField]
    private Follower follower;
 
    void Start()
    {
        follower = GetComponent<Follower>();
        if (follower.Target == null) follower.Target = GameObject.FindWithTag("Player").transform;
    }
}
