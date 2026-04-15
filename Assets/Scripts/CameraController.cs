using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Camera mirrorCamera;

    void Update()
    {
        mirrorCamera.fieldOfView = camera.fieldOfView;
    }
}
