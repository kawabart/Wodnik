using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

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
}
