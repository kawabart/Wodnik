using UnityEngine;

public class TaintController : MonoBehaviour
{
    public SurfaceType taintSurfaceType = null;
    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;
    private Material material;
    void Start()
    {
        if (meshRenderer==null) meshRenderer = GetComponent<SkinnedMeshRenderer>();
        material = meshRenderer.materials[0];
    }
    public void TryTaint(Vector3 position, SurfaceType surface)
    {
        float minimumTaintDistanceSquared = 0.16f;
        if ((transform.position - position).sqrMagnitude > minimumTaintDistanceSquared) return;
        TryTaint(surface);
    }
    public void TryTaint(SurfaceType surface)
    {
        if (surface.surfaceName == "Flesh") material.SetFloat("_SurfaceFleshCut", .3f);
        if (surface.surfaceName == "Water") material.SetFloat("_SurfaceFleshCut", -1f);
    }
    public void Clean()
    {
        material.SetFloat("_SurfaceFleshCut", -1f);
    }
    void Update()
    {
       
    }
}
