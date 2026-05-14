using UnityEngine;

public class WayPointController : MonoBehaviour
{
    public float InteractionTime = 5f;
    public string StateName = "Idle";

    public void StartInteraction()
    {
        //for future use
    }
    public void StopInteraction()
    {
        //for future use
    }
    private void OnDrawGizmos()
    {
        if (StateName == "Idle")
            Gizmos.color = Color.green;
        if (StateName == "AttackIdle")
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, .15f);
    }
}
