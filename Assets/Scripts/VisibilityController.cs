using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    public bool Visible { get; private set; }
    
    void FixedUpdate()
    {
        Visible = CheckVisibility();
    }
    bool CheckVisibility()
    {
        return true;
    }
}
