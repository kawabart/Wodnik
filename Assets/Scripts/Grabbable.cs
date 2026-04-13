using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour, IGrabbable
{
    public UnityEvent onGrab;
    public UnityEvent onLetGo;


    public void Grab(HairController hairController) 
    {
        Debug.Log("I'm grabbed!");
        onGrab.Invoke();
        hairController.Grab(this.GetComponent<Rigidbody>());
    }
    public void LetGo(HairController hairController)
    {
        onLetGo.Invoke();  
    }
}
