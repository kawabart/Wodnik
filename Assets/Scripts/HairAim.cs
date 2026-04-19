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
    }
    void Update()
    {
        if (playerController.targetedGrabObject == null) endPoint.Target = defaultHairPosition;
        else endPoint.Target = playerController.targetedGrabObject.transform;
    }
}
