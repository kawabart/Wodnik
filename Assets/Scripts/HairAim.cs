using UnityEngine;

public class HairAim : MonoBehaviour
{
    [SerializeField] HairGenerator hairGeneratorAiming;
    private PlayerController playerController;
    [SerializeField]
    private Transform defaultHairPosition;

    [SerializeField]
    private Follower endPoint;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        hairGeneratorAiming.middlepoint.transform.parent = null;
        hairGeneratorAiming.endpoint.transform.parent = null;
    }
    void Update()
    {
        if (playerController.targetedGrabObject == null)
        {
            endPoint.Target = defaultHairPosition;
            hairGeneratorAiming.middlepointStiffness = .1f;
            hairGeneratorAiming.dampness = .9f;
            endPoint.SmoothTime = 0.05f;
        }
        else
        {
            endPoint.Target = playerController.targetedGrabObject.transform;
            hairGeneratorAiming.middlepointStiffness = 0.01f;
            hairGeneratorAiming.dampness = .7f;
            endPoint.SmoothTime = 0.15f;
        }
    }
}
