using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour, IGrabbable
{
    public UnityEvent onGrab;
    public UnityEvent onLetGo;

    private EnemyController enemyController;
    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }
    public bool Grab(HairController hairController)
    {
        if (!CanBeGrabbed()) return false;
        Debug.Log("I'm grabbed!");
        onGrab.Invoke();
        hairController.Grab(this.GetComponent<Rigidbody>());
        return true;
    }
    public bool CanBeGrabbed()
    {
        if (enemyController!=null && enemyController.TryBlocking()) return false;
        return true;
    }
    public void LetGo(HairController hairController)
    {
        onLetGo.Invoke();
    }
}
