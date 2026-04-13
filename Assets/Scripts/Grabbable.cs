using UnityEngine;
using UnityEngine.Events;

public class Grabbable : MonoBehaviour, IGrabbable
{
    public UnityEvent onGrab;
    public UnityEvent onLetGo;


    public void Grab(HairController hairController) 
    {
        onGrab.Invoke();
    }
    public void LetGo(HairController hairController)
    {
        onLetGo.Invoke();
    }
}
