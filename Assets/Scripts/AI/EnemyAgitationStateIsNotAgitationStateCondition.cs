using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Enemy agitation state is not AgitationState", story: "[Enemy] agitation state is not [AgitationState]", category: "Conditions", id: "943fafda835d03a161eaf293651e65a7")]
public partial class EnemyAgitationStateIsNotAgitationStateCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Enemy;
    [SerializeReference] public BlackboardVariable<AgitationState> AgitationState;

    public override bool IsTrue()
    {
        return Enemy.Value.GetComponent<AgitationController>().AgitationState != AgitationState.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
