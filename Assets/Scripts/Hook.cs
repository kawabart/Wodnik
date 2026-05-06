using UnityEngine;

/// <summary>
/// Object with trigger collider, that Hookable object can be attached to.
/// </summary>
public class Hook : MonoBehaviour
{
    public Hookable hookedObject = null;
    private bool canHook = true;
    private float timeout = 2;
    private float timer = 0;
    private void Update()
    {
        if (hookedObject != null)
        {
            timer = timeout;
        }
        else if (!canHook)
        {
            timer -= Time.deltaTime;
            if (timer < 0) canHook = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!canHook) return;
        if (other.TryGetComponent<Hookable>(out Hookable hookable))
        {
            hookable.Hook(this);
            canHook = false;
            hookedObject = hookable;
        }
    }
}
