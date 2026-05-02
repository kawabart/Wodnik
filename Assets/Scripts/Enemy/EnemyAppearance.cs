using UnityEditor;
using UnityEngine;

public class EnemyAppearance : MonoBehaviour
{
    [SerializeField] private Mesh[] beards;
    [SerializeField] private Mesh[] hair;
    [SerializeField] private Mesh[] moustaches;
    [SerializeField] private Mesh[] helmets;
    [SerializeField] private SkinnedMeshRenderer beardMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer hairMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer moustacheMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer bodyMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer helmetMeshRenderer;
    [SerializeField] Material[] materials;
    Damageable damageable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageable = GetComponentInParent<Damageable>();
        RandomizeMeshes(beardMeshRenderer, beards);
        RandomizeMeshes(hairMeshRenderer, hair);
        RandomizeMeshes(moustacheMeshRenderer, moustaches);
        RandomizeMeshes(helmetMeshRenderer, helmets);
        Material hairMaterial = materials[Random.Range(0, materials.Length)];
        Material clothesMaterial = materials[Random.Range(0, materials.Length)]; // materials[0];//didn't like them too colorfull
        bodyMeshRenderer.sharedMaterial = clothesMaterial;
        beardMeshRenderer.sharedMaterial = hairMaterial;
        moustacheMeshRenderer.sharedMaterial = hairMaterial;
        hairMeshRenderer.sharedMaterial = hairMaterial;
        helmetMeshRenderer.sharedMaterial = clothesMaterial;
    }
    private void Update()
    {
        if (damageable != null && damageable.health<=1) helmetMeshRenderer.enabled = false;
    }

    private void RandomizeMeshes(SkinnedMeshRenderer meshRenderer, Mesh[] meshes)
    {
        if (!meshRenderer)
            return;

        if (meshes == null || meshes.Length == 0)
        {
            meshRenderer.sharedMesh = null;
            meshRenderer.enabled = false;
            return;
        }

        Mesh mesh = meshes[Random.Range(0, meshes.Length)];

        meshRenderer.sharedMesh = mesh;
        meshRenderer.enabled = mesh;
    }
}
