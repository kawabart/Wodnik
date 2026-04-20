using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Melee Attack", story: "[Agent] performs melee attack", category: "Action", id: "d625a105b4f3a3aa6bf56a46b9775a1b")]
public partial class MeleeAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    protected override Status OnStart()
    {
        var component = Agent.Value.GetComponent<MeleeDamageDealer>();
        if (component == null)
            return Status.Failure;
        var attackPerformed = component.DoImpulseAttack();
        if (!attackPerformed)
            return Status.Failure;
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

