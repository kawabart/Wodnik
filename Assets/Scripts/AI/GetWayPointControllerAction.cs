using System;
using System.Security.Cryptography.X509Certificates;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Way Point Controller", story: "Interact with [component] from [object] using [animator]", category: "Action", id: "88d7707cb0d86cf68c7aca81c4d6e389")]
public partial class GetWayPointControllerAction : Action
{
    [SerializeReference] public BlackboardVariable<WayPointController> Component;
    [SerializeReference] public BlackboardVariable<GameObject> Object;
    [SerializeReference] public BlackboardVariable<Animator> Animator;

    private float timer;
    protected override Status OnStart()
    {
        if (Object.Value == null) return Status.Failure;
        if (Object.Value.TryGetComponent<WayPointController>(out WayPointController wayPointController))
        {
            Component.Value = wayPointController;
            timer = wayPointController.InteractionTime;
            Animator.Value.CrossFade(wayPointController.StateName, 0.01f);
            Component.Value.StartInteraction();
            return Status.Running;
        }
        return Status.Failure;
    }

    protected override Status OnUpdate()
    {
        if (Object.Value == null) return Status.Failure;
        if (Component.Value == null) return Status.Failure;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            
            return Status.Success;
        }
            

        return Status.Running;
    }

    protected override void OnEnd()
    {
        Component.Value.StopInteraction();
        Animator.Value.SetTrigger("EndIdle");
    }
}

