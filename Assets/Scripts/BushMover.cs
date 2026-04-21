using UnityEngine;

public class BushMover : MonoBehaviour
{
    public float timer = 0;
    public float duration = 5;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 10f;
    public Transform[] plants;
    private Quaternion[] originalRotations;
    private float startTime;

    void Start()
    {
        InitializePlants();
    }
    void Update()
    {
        //to do: add culling for squared distance to player
        if (timer > 0)
        {
            AnimatePlants(timer);
            timer -= Time.deltaTime;
        }
    }
    void InitializePlants()
    {
        originalRotations = new Quaternion[plants.Length];
        for (int i = 0; i < plants.Length; i++)
        {
            originalRotations[i] = plants[i].transform.localRotation;
        }
    }
    void AnimatePlants(float timer)
    {
        for (int i = 0; i < plants.Length; i++)
        {
            float normalizedTime = timer / duration;
            normalizedTime *= normalizedTime * normalizedTime * normalizedTime;

            float amplitude = shakeAmount * normalizedTime;
            float angle = Mathf.Sin((Time.time - startTime) * shakeSpeed) * amplitude;

            Quaternion shakeRot = Quaternion.Euler(angle, 0f, 0f);

            plants[i].transform.localRotation = originalRotations[i] * shakeRot;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            timer = duration;
            startTime = Time.time;
        }
    }
}
