using UnityEngine;

public class TaintController : MonoBehaviour
{
    [SerializeField]
    private float taintDistance = 1;
    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;
    private Material material;
    void Start()
    {
        if (meshRenderer == null) meshRenderer = GetComponent<SkinnedMeshRenderer>();
        material = meshRenderer.materials[0];
    }
    public void TryTaint(Vector3 position, SurfaceType surface)
    {
        float minimumTaintDistanceSquared = taintDistance * taintDistance;
        if ((transform.position - position).sqrMagnitude > minimumTaintDistanceSquared) return;
        TryTaint(surface);
    }
    public void TryTaint(SurfaceType surface)
    {
        if (surface.surfaceName == "Flesh")
        {
            float fleshCut = Mathf.Max(0,material.GetFloat("_SurfaceFleshCut"));
            fleshCut =Mathf.Max(0, fleshCut);
            material.SetFloat("_SurfaceFleshCut", Mathf.Min(.7f, fleshCut + .2f));
        }
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
