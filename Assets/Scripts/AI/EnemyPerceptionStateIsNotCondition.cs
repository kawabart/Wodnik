using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Enemy perception state is not", story: "[Enemy] perception state is not [PerceptionState]", category: "Conditions", id: "3b8c846cd39752d5620905d40f222c16")]
public partial class EnemyPerceptionStateIsNotCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<EnemyPerceptionState> PerceptionState;

    public override bool IsTrue()
    {
        return Enemy.Value.GetComponent<EnemyPerception>().PerceptionState != PerceptionState.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
