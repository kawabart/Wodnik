using UnityEngine;
using UnityEngine.Events;

//spawns object based on Infamy
public class InfamySpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Transform Pivot;
    public float InfamyTreshold = 50f;
    public float InfamyRandomOffset = 0;
    public UnityEvent onInfamyHightEnough;
    private void Start()
    {
        if (Pivot != null) Pivot.gameObject.SetActive(false);
        if (GameManager.Instance.CurrentChaos < InfamyTreshold + Random.Range(0, InfamyRandomOffset)) return;
        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        onInfamyHightEnough.Invoke();
        if (objectsToSpawn.Length < 1 || objectsToSpawn[randomIndex] == null) return;
        GameObject spawnedObject = Instantiate(objectsToSpawn[randomIndex], transform);

        if (Pivot == null) return;
        spawnedObject.transform.position = Pivot.position;
        spawnedObject.transform.rotation = Pivot.rotation;


    }
}
