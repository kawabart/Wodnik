using UnityEngine;

public class TrailSpawner : MonoBehaviour
{
    [SerializeField]
    private TrailPiece trailPrefab;

    [SerializeField]
    private float cooldown = 1;

    private float timer;

    private TrailPiece lastPiece;

    [SerializeField] private float minDistance = .5f;
    private Vector3 lastSpawnPosition = Vector3.zero;

    void Update()
    {
        timer -=Time.deltaTime;
        if (timer < 0 && Vector3.Distance(transform.position, lastSpawnPosition) > minDistance)
        {
            SpawnTrailPiece();
            timer = cooldown;
            lastSpawnPosition = transform.position;
        }
    }
    void SpawnTrailPiece()
    {
        TrailPiece newPiece = Instantiate(trailPrefab, transform.position, transform.rotation);
        newPiece.nextPosition = this.transform;
        if (lastPiece != null) lastPiece.nextPosition = newPiece.transform;

        lastPiece = newPiece;
    }
}
