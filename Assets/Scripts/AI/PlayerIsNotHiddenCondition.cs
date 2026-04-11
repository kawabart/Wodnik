using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Player is not hidden", story: "[Player] is not hidden", category: "Conditions", id: "8fae171db06a06248efbc89a02e4b017")]
public partial class PlayerIsNotHiddenCondition : Condition
{
    [SerializeReference] public BlackboardVariable<PlayerController> Player;

    public override bool IsTrue()
    {
        return !Player.Value.Hidden;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
