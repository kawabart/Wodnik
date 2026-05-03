using UnityEngine;

public class Timer : MonoBehaviour
{
    public float time = 0f;
    public bool enable = false;
    void Update()
    {
        if (enable)
        {
            time += Time.deltaTime;
        }
    }

    public void Reset()
    {
        time = 0f;
        enable = false;
    }

    public void Start()
    {
        enable = true;
    }
}
