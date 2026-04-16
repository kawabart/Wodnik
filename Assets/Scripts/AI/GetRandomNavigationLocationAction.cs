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

    protected override Status OnStart()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * Range.Value;
        randomDirection.y = 0.0f;
        var randomPosition = Location.Value + randomDirection;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, Range.Value, 1);
        RandomLocation.Value = hit.position;

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
