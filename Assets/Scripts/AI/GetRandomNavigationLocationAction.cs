using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get random navigation location", story: "Get [RandomLocation] for [Enemy]", category: "Wodnik", id: "055a7fd43fee54c1207949fb97947664")]
public partial class GetRandomNavigationLocationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<Vector3> Location;
    [SerializeReference] public BlackboardVariable<float> Range;
    [SerializeReference] public BlackboardVariable<Vector3> RandomLocation;
    [SerializeReference] public BlackboardVariable<int> Attempts = new BlackboardVariable<int>(5);

    protected override Status OnStart()
    {
        NavMeshHit hit = new NavMeshHit();
        for (int i = 0; i < Attempts.Value; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * Range.Value;
            randomDirection.y = 0.0f;
            var randomPosition = Location.Value + randomDirection;
            if (NavMesh.SamplePosition(randomPosition, out hit, 0, NavMesh.AllAreas))
                break;
        }

        if (!hit.hit)
            return Status.Failure;

        NavMeshPath path = new NavMeshPath();
        var target = hit.position;
        while (true)
        {
            NavMesh.CalculatePath(Location.Value, target, NavMesh.AllAreas, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                break;
            }
            else if (path.status == NavMeshPathStatus.PathInvalid)
            {
                // target position is not reachable
                return Status.Failure;
            }
            else if (path.status == NavMeshPathStatus.PathPartial)
            {
                // try reaching pre-final path point
                target = path.corners[^2];
            }
        }

        RandomLocation.Value = target;
        return Status.Success;
    }
}
