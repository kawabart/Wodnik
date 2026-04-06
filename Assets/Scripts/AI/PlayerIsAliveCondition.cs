using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "PlayerIsAlive", story: "[Player] is alive", category: "Conditions", id: "3b8c846cd39752d5620905d40f222c16")]
public partial class PlayerIsAliveCondition : Condition
{
    [SerializeReference] public BlackboardVariable<PlayerController> Player;

    public override bool IsTrue()
    {
        return Player.Value.IsAlive;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
