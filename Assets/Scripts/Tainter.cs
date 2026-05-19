using UnityEngine;

public class Tainter : MonoBehaviour
{
    private SurfaceType surfaceToApply;
 
    void Start()
    {
        surfaceToApply = GetComponent<Surface>().type;
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TaintController>(out TaintController taint))
        {
            taint.TryTaint(surfaceToApply);
        }
    }
}
