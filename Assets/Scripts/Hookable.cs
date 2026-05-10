using UnityEngine;

/// <summary>
/// Object that can be attached to a Hook object.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Hookable : MonoBehaviour
{
    private bool Hooked = false;
    private Rigidbody rb;
    private Hook currentHook = null;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void UnHook()
    {
        if (currentHook != null) currentHook.hookedObject = null;
        rb.isKinematic = false;
        Hooked = false;
    }
    public void Hook(Hook hook)
    {
        rb.isKinematic = true;
        transform.position = hook.transform.position;
        currentHook = hook;
        if (this.TryGetComponent<IGrabbable>(out var grabbable))
            grabbable.LetGo();
        Hooked = true;
    }
}
