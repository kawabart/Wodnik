using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Take next waypoint", story: "Take next [Waypoint] from [Waypoints]", category: "Wodnik", id: "76af6f9cd4940964ca538dd70ff8c1bf")]
public partial class TakeNextWaypointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Waypoint;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;

    protected override Status OnStart()
    {
        if (Waypoints.Value == null || Waypoints.Value.Count == 0) return Status.Failure;

        if (Waypoint.Value == null)
        {
            Waypoint.Value = Waypoints.Value[0];
        }
        else
        {
            var index = Waypoints.Value.IndexOf(Waypoint.Value);
            Waypoint.Value = Waypoints.Value[(index + 1) % Waypoints.Value.Count];
        }
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}
