using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Enemy Perception State", story: "Get [Enemy] perception state", category: "Wodnik", id: "664fda92c57fa3f3097399f90337372f")]
public partial class GetPerceptionStateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<EnemyPerceptionState> PerceptionState;

    protected override Status OnStart()
    {
        var perception = Enemy.Value.GetComponent<EnemyPerception>();
        if (perception == null) return Status.Failure;
        PerceptionState.Value = perception.PerceptionState;
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
