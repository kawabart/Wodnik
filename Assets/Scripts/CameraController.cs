using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Follower follower;
 
    void Start()
    {
        follower = GetComponent<Follower>();
        if (follower.Target == null) follower.Target = GameObject.FindWithTag("Player").transform;
    }
}
