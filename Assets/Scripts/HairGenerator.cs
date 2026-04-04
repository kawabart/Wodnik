using UnityEngine;

public class HairGenerator : MonoBehaviour
{
    public Transform startpoint;
    public Transform middlepoint;
    public Transform endpoint;

    public LineRenderer lineRenderer;

    public Vector3 middlepointVelocity = Vector3.zero;
    public float middlepointStiffness = .005f;
    public float dampness = .1f;
    public int resolution = 20;



    void CalculateMidlepoint()
    {
        Vector3 middlepointTargetPosition = Vector3.LerpUnclamped(startpoint.position, endpoint.position, .5f);
        
        middlepointVelocity += (middlepointTargetPosition - middlepoint.transform.position) * middlepointStiffness;
        middlepointVelocity *= dampness;
        middlepoint.transform.position += middlepointVelocity;
       
    }

    void GenerateStrand()
    {
        lineRenderer.positionCount = resolution;

        Vector3 A = startpoint.position;
        Vector3 B = middlepoint.position;
        Vector3 C = endpoint.position;

        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            Vector3 pos = Bezier(A, B, C, t);
            lineRenderer.SetPosition(i, pos);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMidlepoint();
        GenerateStrand();
    }

    Vector3 Bezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return Mathf.Pow(1 - t, 2) * A +
               2 * (1 - t) * t * B +
               Mathf.Pow(t, 2) * C;
    }
}   
