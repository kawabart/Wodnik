using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get suggested move speed", story: "Get [Object] [MoveSpeed]", category: "Wodnik", id: "97f96366f9d34d51331696cbd75b354a")]
public partial class GetSuggestedMoveSpeedAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;

    protected override Status OnStart()
    {
        var component = Object.Value.GetComponent<AgitationController>();
        if (component == null)
            return Status.Failure;
        MoveSpeed.Value = Object.Value.GetComponent<AgitationController>().SuggestedSpeed;
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
