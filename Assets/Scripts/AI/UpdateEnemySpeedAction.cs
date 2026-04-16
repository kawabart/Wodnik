using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Update Enemy speed", story: "Update [Enemy] speed", category: "Wodnik", id: "4b0f805318c529108bc66dacc968b155")]
public partial class UpdateEnemySpeedAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;

    protected override Status OnStart()
    {
        Enemy.Value.GetComponent<NavMeshAgent>().speed = Enemy.Value.GetComponent<AgitationController>().SuggestedSpeed;
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
