using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Show Enemy UI", story: "[Enemy] shows UI [state]", category: "Wodnik", id: "d2ed54851f951b31664eb3806ce414b8")]
public partial class ShowEnemyUiAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<EnemyAIState> State;

    protected override Status OnStart()
    {
        var uiComponent = Enemy.Value.GetComponentInChildren<EnemyUI>();
        if (uiComponent != null)
        {
            uiComponent.State = State;
            return Status.Success;
        }
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

