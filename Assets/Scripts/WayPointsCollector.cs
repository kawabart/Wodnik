using System.Collections.Generic;
using UnityEngine;
using Unity.Behavior;

public class WayPointsCollector : MonoBehaviour
{
    [SerializeField] private BehaviorGraphAgent behaviorTreeRunner;
    [SerializeField] private string blackboardVariableName = "Waypoints";
    [SerializeField]
    private List<GameObject> waypoints;

    [ContextMenu("Collect Waypoints (Editor)")]

    private void Awake()
    {
        CollectWaypoints();
    }
    private void CollectWaypoints()
    {
        waypoints = new();

        foreach (Transform child in transform)
        {
            waypoints.Add(child.gameObject);
        }

        if (behaviorTreeRunner == null || behaviorTreeRunner.BlackboardReference == null)
        {
            Debug.LogWarning("Missing BehaviorGraphAgent or Blackboard");
            return;
        }

        behaviorTreeRunner.BlackboardReference.SetVariableValue(
            blackboardVariableName,
            waypoints//.ToArray()
        );

        Debug.Log($"Collected {waypoints.Count} waypoints");
    }
}
