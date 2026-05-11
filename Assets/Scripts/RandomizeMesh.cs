using UnityEngine;

public class RandomizeMesh : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private bool randomizeOnStart = true;

    private void Start()
    {
        if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
        if (randomizeOnStart) PickRandomMesh();
    }
    public void PickRandomMesh()
    {
        if (meshFilter == null) return;
        if (meshes.Length == 0) return;
        Mesh randomMesh = meshes[Random.Range(0,meshes.Length)];
        meshFilter.sharedMesh = randomMesh;
    }
}
