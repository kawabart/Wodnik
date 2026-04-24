using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    public UnityEvent onTrigger;
    public bool oneTime = false;
    public Rigidbody objectToTrigger = null;

    private void OnTriggerEnter(Collider other)
    {
        if (objectToTrigger != null)
        {
            if (other.attachedRigidbody == objectToTrigger) EventTriggered();
        }
        else
        {
            EventTriggered();
        }
    }
    void EventTriggered()
    {
        onTrigger.Invoke();
        if (oneTime) this.enabled = false;
    }
}
