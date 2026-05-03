using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get agitation state", story: "Get [Enemy] [AgitationState]", category: "Wodnik", id: "6cecc4be74f131fa1eecc94024458aeb")]
public partial class GetAgitationStateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<AgitationState> AgitationState;

    protected override Status OnStart()
    {
        var enemyController = Enemy.Value.GetComponent<AgitationController>();
        if (enemyController == null) return Status.Failure;
        AgitationState.Value = enemyController.AgitationState;
        return Status.Success;
    }
}
