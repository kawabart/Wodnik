using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get LastPlayerPosition seen by Enemy", story: "Get [LastPlayerPosition] seen by [Enemy]", category: "Action", id: "53d20c829f03ef3d09efadea22d01ed3")]
public partial class GetLastPlayerPositionSeenByEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> LastPlayerPosition;
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    protected override Status OnStart()
    {
        var component = Enemy.Value.GetComponent<EnemyPerception>();
        if (component == null)
            return Status.Failure;
        var position = component.LastPlayerPosition;
        if (!position.HasValue)
            return Status.Failure;
        LastPlayerPosition.Value = position.Value;
        return Status.Success;
    }
}
