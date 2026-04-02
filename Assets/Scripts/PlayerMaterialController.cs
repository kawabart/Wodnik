using UnityEngine;

public class PlayerMaterialController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    private SkinnedMeshRenderer meshRenderer;
    private Color originalEmissionColor;

    private void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (meshRenderer != null)
        {
            originalEmissionColor = meshRenderer.materials[0].GetColor("_EmissionColor");
        }
    }
    private void Update()
    {
        UpdateEmission();
    }
    void UpdateEmission()
    {
        if (meshRenderer == null) return;

        Color targetColor = playerController.Hidden ? originalEmissionColor * 0.5f : originalEmissionColor;

        meshRenderer.materials[0].SetColor("_EmissionColor", targetColor);

        //meshRenderer.material.EnableKeyword("_EMISSION");
    }
}
