using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    public PlayerController playerController;

    public void ApplyPushForce()
    {
        playerController.ApplyPushForce();
    }
 
    public void EndPush()
    {
        playerController.EndPush();
    }
 
    public void KillTakedownTarget()
    {
        playerController.KillTakedownTarget();
    }
}
