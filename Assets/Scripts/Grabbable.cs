using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour, IGrabbable
{
    public UnityEvent onGrab;
    public UnityEvent onLetGo;

    private EnemyController enemyController;
    private Rigidbody rigidBody;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    public bool Grab(HairController hairController)
    {
        if (!CanBeGrabbed()) return false;

        Debug.Log("I'm grabbed!");
        onGrab.Invoke();
        hairController.Grab(rigidBody);
        return true;
    }

    public bool CanBeGrabbed()
    {
        return enemyController == null || !enemyController.TryBlocking();
    }

    public void LetGo(HairController hairController)
    {
        onLetGo.Invoke();
        hairController.LetGo();
    }
}
