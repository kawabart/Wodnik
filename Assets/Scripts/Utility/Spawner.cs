using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectToSpawn;

    public void Spawn()
    {
        Instantiate(gameObjectToSpawn,transform.position, transform.rotation);
    }
}
