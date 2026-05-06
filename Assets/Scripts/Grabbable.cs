using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour, IGrabbable
{
    public UnityEvent onGrab;
    public UnityEvent onLetGo;

    private EnemyController enemyController;
    private HairController grabbingHairController = null;
    private Rigidbody rb;
    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody>();
    }
  
    public bool Grab(HairController hairController)
    {
        if (!CanBeGrabbed()) return false;
        grabbingHairController = hairController;
        Debug.Log("I'm grabbed!");
        onGrab.Invoke();
        hairController.Grab(rb);
        return true;
    }
  
    public bool CanBeGrabbed()
    {
        if (enemyController != null && enemyController.TryBlocking()) return false;
        return true;
    }
 
    public void ForceLetGo()
    {
        if (grabbingHairController == null) return;
        grabbingHairController.LetGo();
    }
    public void LetGo()
    {
        grabbingHairController = null;
        onLetGo.Invoke();
    }
}
