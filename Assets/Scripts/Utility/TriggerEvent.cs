using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    public UnityEvent onTrigger;
    public UnityEvent onTriggerExit;
    public bool oneTime = false;
    public Rigidbody objectToTrigger = null;
    public LayerMask layerMask = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) == 0) return;
        if (objectToTrigger != null)
        {
            if (other.attachedRigidbody == objectToTrigger) EventTriggered();
        }
        else
        {
            EventTriggered();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask) == 0) return;
        if (objectToTrigger != null)
        {
            if (other.attachedRigidbody == objectToTrigger) EventTriggered();
        }
        else
        {
            ExitEventTriggered();
        }
    }

    void EventTriggered()
    {
        if (!enabled) return;
        onTrigger.Invoke();
        if (oneTime) this.enabled = false;
    }
    void ExitEventTriggered()
    {
        if (!enabled) return;
        onTriggerExit.Invoke();
    }
}
