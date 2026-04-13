using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Player Position", story: "Get [Player] [Position]", category: "Wodnik", id: "53d20c829f03ef3d09efadea22d01ed3")]
public partial class GetPlayerLastPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<PlayerController> Player;
    [SerializeReference] public BlackboardVariable<Vector3> Position;

    protected override Status OnStart()
    {
        Position.Value = Player.Value.transform.position;
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
