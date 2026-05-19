using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Enemy state", story: "Get [Enemy] [State]", category: "Wodnik", id: "8c87a5a5ecaef44b11775816b5bd49c7")]
public partial class GetEnemyStateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<EnemyState> State;

    protected override Status OnStart()
    {
        var enemyController = Enemy.Value.GetComponent<EnemyController>();
        if (enemyController == null) return Status.Failure;
        State.Value = enemyController.CurrentState;
        return Status.Success;
    }
}

