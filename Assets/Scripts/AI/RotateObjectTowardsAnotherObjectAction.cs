using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rotate Object towards AnotherObject", story: "Rotate [Object] towards [AnotherObject]", category: "Wodnik", id: "f61ef4942ea6ef8e80ce87465c692f63")]
public partial class RotateObjectTowardsAnotherObjectAction : Action
{
    [SerializeReference]
    public BlackboardVariable<GameObject> Object;

    [SerializeReference]
    public BlackboardVariable<GameObject> AnotherObject;

    [SerializeReference]
    public BlackboardVariable<float> AngularSpeed = new BlackboardVariable<float>(360.0f);

    [SerializeReference]
    public BlackboardVariable<float> TargetAngle = new BlackboardVariable<float>(1.0f);

    private Vector3 forward, up;

    protected override Status OnStart()
    {
        forward = (AnotherObject.Value.transform.position - Object.Value.transform.position).normalized;
        up = Object.Value.transform.up;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var newRotation = Vector3.RotateTowards(Object.Value.transform.forward, forward, Mathf.Deg2Rad * AngularSpeed * Time.deltaTime, 0);
        Object.Value.GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(newRotation, up));
        if (Vector3.Angle(newRotation, forward) <= TargetAngle)
        {
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
    }
}

