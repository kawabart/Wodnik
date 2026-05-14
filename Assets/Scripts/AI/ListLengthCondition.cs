using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "List length", story: "[List] length is [Compared] to [Number]", category: "Conditions", id: "a2c1c18cacfe3909ee116ce08b5f6262")]
public partial class ListLengthCondition : Condition
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> List;
    [Comparison(comparisonType: ComparisonType.All)]
    [SerializeReference] public BlackboardVariable<ConditionOperator> Compared;
    [SerializeReference] public BlackboardVariable<int> Number = new(0);

    public override bool IsTrue()
    {
        if (List.Value == null) return false;
        return List.Value.Count> Number.Value;
    }
}
