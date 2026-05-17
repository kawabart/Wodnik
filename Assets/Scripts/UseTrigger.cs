using UnityEngine;
using UnityEngine.Events;

public class UseTrigger : MonoBehaviour
{
    public bool IsUsable = true;
    public UnityEvent onUse;
    public void StartUsing()
    {
        onUse.Invoke();
    }
}
