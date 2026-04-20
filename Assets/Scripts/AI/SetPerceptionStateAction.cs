using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Enemy Perception State", story: "Set [Enemy] perception [PerceptionState]", category: "Wodnik", id: "08b026550cf3196b0668b2e879a4ba13")]
public partial class SetPerceptionStateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<EnemyPerceptionState> PerceptionState;

    protected override Status OnStart()
    {
        var perception = Enemy.Value.GetComponent<EnemyPerception>();
        if (perception == null) return Status.Failure;
        perception.PerceptionState = PerceptionState.Value;
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
