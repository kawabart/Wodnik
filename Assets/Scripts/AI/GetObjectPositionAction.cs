using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Object Position", story: "Get [actor] [position]", category: "Wodnik", id: "5db28bffc3ae3f6c60194fb5169f0e04")]
public partial class GetObjectPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Actor;
    [SerializeReference] public BlackboardVariable<Vector3> Position;

    protected override Status OnStart()
    {
        //To do: check if Position and Actor are not null, and return Status.Failure otherwise
        Position.Value = Actor.Value.transform.position;
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

