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

    private MeleeDamageDealer damageDealer;
    protected override Status OnStart()
    {
        damageDealer = Agent.Value.GetComponent<MeleeDamageDealer>();
        if (damageDealer == null)
            return Status.Failure;
        damageDealer.StartAttack();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (damageDealer == null) return Status.Failure;
        if (damageDealer.isAttacking) return Status.Running;
        Debug.Log("Attack running");
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

