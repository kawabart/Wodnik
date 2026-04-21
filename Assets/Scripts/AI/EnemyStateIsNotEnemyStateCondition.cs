using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Enemy State is not EnemyState", story: "[Enemy] State is not [EnemyState]", category: "Conditions", id: "e07589913028e1bcf731b9ad976495e7")]
public partial class EnemyStateIsNotEnemyStateCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<EnemyState> EnemyState;

    public override bool IsTrue()
    {
        return Enemy.Value.GetComponent<EnemyController>().CurrentState != EnemyState.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
