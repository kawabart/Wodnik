using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rotate GameObject towards Position", story: "Rotate [Object] towards [Position]", category: "Wodnik", id: "cd5ddb437386ac2619fe003d20a31d5d")]
public partial class RotateGameObjectTowardsPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<Vector3> Position;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var forward = Position.Value - Object.Value.transform.position;
        var up = Object.Value.transform.up;
        Object.Value.GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(forward, up));
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

